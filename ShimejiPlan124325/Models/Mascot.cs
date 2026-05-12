using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShimejiPlan124325.Models
{
    public class Mascot : INotifyPropertyChanged
    {
        // доступ ссылки на поля с данными
        private string _name;
        private string _jarPath;
        private bool _isRunning;
        private int? _processId;

        public string Name // название Shimeji
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); } // широкие подробные росписи изза реализации с приватными полями
        }

        public string JarPath // полный путь к Shimeji
        {
            get => _jarPath;
            set { _jarPath = value; OnPropertyChanged(nameof(JarPath)); }
        }

        public bool IsRunning // true, если процесс запущен типа обновления
        {
            get => _isRunning;
            set
            {
                if (_isRunning == value) return;
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
                OnPropertyChanged(nameof(Status)); // оповещения для таблицы
            }
        }

        [JsonIgnore]
        public int? ProcessId
        {
            get => _processId;
            set { _processId = value; OnPropertyChanged(nameof(ProcessId)); }
        }

        // Вычисляемое свойство без set
        public string Status => IsRunning ? "Запущен" : "Остановлен"; // пропись привязанного поля без гет сет

        public event PropertyChangedEventHandler PropertyChanged; // стандарт событие для прослушивания данных

        protected void OnPropertyChanged(string propertyName = null) // оповещение об обновлении
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class MascotStorage
    {
        static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ShimejiManager", "mascots.json");

        public static List<Mascot> Load()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<Mascot>>(json) ?? new List<Mascot>();
            }
            return new List<Mascot>();
        }

        public static void Save(IEnumerable<Mascot> mascots)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            var json = JsonSerializer.Serialize(mascots, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }

    public class MascotProcessManager
    {
        /// <summary>
        /// Запускает процесс для указанного маскота.
        /// </summary>
        /// <param name="m">Объект маскота, содержащий данные для запуска</param>
        /// <exception cref="ArgumentNullException">Если mascot равен null</exception>
        /// <exception cref="InvalidOperationException">Если путь к исполняемому файлу не указан</exception>
        // Хранит соответствие маскота → процесс, чтобы можно было остановить
        private readonly Dictionary<Mascot, Process> _runningProcesses = new Dictionary<Mascot, Process>();

        // WinAPI для поиска окна и отправки сообщения
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private const uint WM_CLOSE = 0x0010;
        private const string SHIMEJI_WINDOW_CLASS = "SunAwtFrame";

        /// <summary>
        /// Запускает Java-процесс для маскота и начинает отслеживание его состояния.
        /// </summary>
        public void Start(Mascot mascot)
        {
            if (mascot.IsRunning)
                throw new InvalidOperationException($"Маскот '{mascot.Name}' уже запущен.");

            var psi = new ProcessStartInfo("java", $"-jar \"{mascot.JarPath}\"")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false
            };

            Process proc;
            try
            {
                proc = Process.Start(psi);
                if (proc == null)
                    throw new Exception("Не удалось запустить процесс (Process.Start вернул null).");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка запуска Java: {ex.Message}", ex);
            }

            mascot.ProcessId = proc.Id;
            mascot.IsRunning = true;

            // Подписываемся на событие завершения процесса
            proc.EnableRaisingEvents = true;
            proc.Exited += (sender, args) =>
            {
                // Событие приходит из фонового потока, поэтому обновление UI должно быть в потоке UI
                System.Windows.Forms.Application.OpenForms[0]?.Invoke((Action)(() =>
                {
                    mascot.IsRunning = false;
                    mascot.ProcessId = null;
                }));

                lock (_runningProcesses)
                {
                    _runningProcesses.Remove(mascot);
                }
            };

            lock (_runningProcesses)
            {
                _runningProcesses[mascot] = proc;
            }
        }

        /// <summary>
        /// Останавливает маскота: сначала отправляет WM_CLOSE окну, через 3 секунды принудительно завершает процесс.
        /// </summary>
        public void Stop(Mascot mascot)
        {
            if (!mascot.IsRunning || mascot.ProcessId == null)
                return;

            Process proc = null;
            lock (_runningProcesses)
            {
                if (_runningProcesses.TryGetValue(mascot, out proc))
                {
                    // Процесс всё ещё отслеживается
                }
                else
                {
                    // Возможно, процесс уже завершился, но флаг не сброшен
                    mascot.IsRunning = false;
                    mascot.ProcessId = null;
                    return;
                }
            }

            try
            {
                if (!proc.HasExited)
                {
                    // Найти главное окно Shimeji по PID
                    IntPtr hWnd = FindShimejiWindowByProcessId(proc.Id);
                    if (hWnd != IntPtr.Zero)
                    {
                        // Мягкое закрытие окна
                        PostMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        // Ждём до 3 секунд
                        if (!proc.WaitForExit(3000))
                        {
                            // Если не завершился, принудительно
                            proc.Kill();
                        }
                    }
                    else
                    {
                        // Окно не найдено – завершаем процесс напрямую
                        proc.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                // Игнорируем ошибки: процесс уже мог быть закрыт
                Debug.WriteLine($"Ошибка при остановке маскота '{mascot.Name}': {ex.Message}");
            }
            finally
            {
                mascot.IsRunning = false;
                mascot.ProcessId = null;
                lock (_runningProcesses)
                {
                    _runningProcesses.Remove(mascot);
                }
                proc?.Dispose();
            }
        }

        /// <summary>
        /// Останавливает все запущенные маскоты.
        /// </summary>
        public void StopAll()
        {
            List<Mascot> toStop;
            lock (_runningProcesses)
            {
                toStop = _runningProcesses.Keys.ToList();
            }
            foreach (var mascot in toStop)
            {
                Stop(mascot);
            }
        }

        /// <summary>
        /// Ищет главное окно Shimeji по идентификатору процесса.
        /// </summary>
        private IntPtr FindShimejiWindowByProcessId(int processId)
        {
            IntPtr foundWindow = IntPtr.Zero;
            EnumWindows((hWnd, lParam) =>
            {
                // Проверяем класс окна
                StringBuilder className = new StringBuilder(256);
                GetClassName(hWnd, className, className.Capacity);
                if (className.ToString() == SHIMEJI_WINDOW_CLASS)
                {
                    // Сверяем PID процесса
                    GetWindowThreadProcessId(hWnd, out uint pid);
                    if (pid == processId)
                    {
                        foundWindow = hWnd;
                        return false; // Прекращаем перебор
                    }
                }
                return true; // Продолжаем перебор
            }, IntPtr.Zero);

            return foundWindow;


        }


    }
}