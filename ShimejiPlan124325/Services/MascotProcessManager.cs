using ShimejiPlan124325.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShimejiPlan124325.Services
{
    public class MascotProcessManager
    {
        private readonly Dictionary<Mascot, Process> _runningProcesses = new Dictionary<Mascot, Process>();

        [DllImport("user32.dll", SetLastError = true)]// WinAPI для поиска окна и отправки сообщения
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        internal const uint WM_CLOSE = 0x0010;
        internal const string SHIMEJI_WINDOW_CLASS = "SunAwtFrame";
        public void Start(Mascot mascot)
        {
            if (mascot.IsRunning)
                throw new InvalidOperationException($"Маскот '{mascot.Name}' уже запущен.");

            var psi = new ProcessStartInfo("java", $"-jar \"{mascot.JarPath}\"")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                WorkingDirectory = Path.GetDirectoryName(mascot.JarPath)   // ← добавить эту строку
            };

            Process? proc;
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

            proc.EnableRaisingEvents = true;
            proc.Exited += (sender, args) =>
            {
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
        public void Stop(Mascot mascot)
        {
            if (!mascot.IsRunning || mascot.ProcessId == null)
                return;

            Process? proc = null;
            lock (_runningProcesses)
            {
                if (_runningProcesses.TryGetValue(mascot, out proc))
                {
                    // Процесс всё ещё отслеживается
                }
                else
                {
                    mascot.IsRunning = false; // Возможно, процесс уже завершился, но флаг не сброшен
                    mascot.ProcessId = null;
                    return;
                }
            }

            try
            {
                if (!proc.HasExited)
                {
                    IntPtr hWnd = IntPtr.Zero;
                    var result = ShimejiWindowFinder.FindWindowByMascot(mascot);// Найти главное окно Shimeji по PID
                    if (result != null)
                        hWnd = result.Value.hWnd;

                    if (hWnd != IntPtr.Zero)
                    {
                        PostMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        if (!proc.WaitForExit(3000))
                        {
                            proc.Kill();
                        }
                    }
                    else
                    {
                        proc.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
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


    }
}
