using ShimejiPlan124325.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace ShimejiPlan124325
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        BindingList<Mascot> mascots = new BindingList<Mascot>(); // создаем для показа
        private readonly MascotProcessManager _processManager = new MascotProcessManager(); // показ с настройками

        BindingList<ScheduledNotification> notifications = new BindingList<ScheduledNotification>(); // показ уведомлений
        private BindingList<ScheduledNotification> _notifications; //еще к уведам

        public MainForm()
        {
            InitializeComponent();

            // запуск DataGridView на первой странице с шимеджи

            mascots = new BindingList<Mascot>(MascotStorage.Load());
            // Если нужно, подписываемся на событие ListChanged для автосохранения
            mascots.ListChanged += (s, e) => MascotStorage.Save(mascots);
            dgvMascots.DataSource = mascots;

            dgvMascots.AutoGenerateColumns = false; // отключение автогенерации столбов 
            dgvMascots.DataSource = mascots;

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn(); // поле имя
            colName.Name = "colName";
            colName.HeaderText = "Имя";
            colName.DataPropertyName = "Name";
            dgvMascots.Columns.Add(colName);

            DataGridViewTextBoxColumn colPath = new DataGridViewTextBoxColumn(); // поле путь
            colPath.Name = "colPath";
            colPath.HeaderText = "Путь";
            colPath.DataPropertyName = "JarPath";
            dgvMascots.Columns.Add(colPath);


            DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn(); // поле статус
            colStatus.Name = "colStatus";
            colStatus.HeaderText = "Статус";
            colStatus.DataPropertyName = "Status";
            dgvMascots.Columns.Add(colStatus);

            DataGridViewTextBoxColumn colProcessId = new DataGridViewTextBoxColumn(); // скрытое поле для хранения ProcessId
            colProcessId.Name = "colProcessId";
            colProcessId.HeaderText = "Process ID"; // его не видно но пусть будет
            colProcessId.Visible = false;          // скрытие ее
            colProcessId.DataPropertyName = "ProcessId";
            dgvMascots.Columns.Add(colProcessId);

            // настройка panel на первой странице на главной форме


            // настройка  DataGridView на второй странице с задачами

            dgvNotifications.DataSource = notifications;
            _notifications = new BindingList<ScheduledNotification>(NotificationStorage.Load());
            _notifications.ListChanged += (s, e) => NotificationStorage.Save(_notifications);
            dgvNotifications.DataSource = _notifications;
            dgvNotifications.AutoGenerateColumns = false; // Отключаем авто-генерацию, если будете привязывать данные

            // Колонка "Время" с форматом
            DataGridViewTextBoxColumn colTime = new DataGridViewTextBoxColumn();
            colTime.Name = "colTime";
            colTime.DataPropertyName = "Time";
            colTime.HeaderText = "Время";
            colTime.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            // Если источник данных ожидает DateTime, формат применится автоматически
            // colTime.DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.InvariantCulture;
            dgvNotifications.Columns.Add(colTime);

            // Колонка "Сообщение"
            DataGridViewTextBoxColumn colMessage = new DataGridViewTextBoxColumn();
            colMessage.Name = "colMessage";
            colMessage.DataPropertyName = "Message";
            colMessage.HeaderText = "Сообщение";
            dgvNotifications.Columns.Add(colMessage);

            // Колонка "Маскот"
            DataGridViewTextBoxColumn colTarget = new DataGridViewTextBoxColumn();
            colTarget.Name = "colTarget";
            colTarget.DataPropertyName = "Target";
            colTarget.HeaderText = "Маскот";
            dgvNotifications.Columns.Add(colTarget);

            // Колонка "Повтор"
            DataGridViewTextBoxColumn colRepeat = new DataGridViewTextBoxColumn();
            colRepeat.Name = "colRepeat";
            colRepeat.DataPropertyName = "Repeat";
            colRepeat.HeaderText = "Повтор";
            dgvNotifications.Columns.Add(colRepeat);

        }

        private void Addbtn_Click(object sender, EventArgs e)
        {
            using (var openDlg = new OpenFileDialog())
            {
                openDlg.Filter = "Jar-файлы (*.jar)|*.jar|Все файлы (*.*)|*.*";
                openDlg.Title = "Выберите исполняемый Jar Shimeji";
                if (openDlg.ShowDialog() != DialogResult.OK)
                    return;

                string sourceJarPath = openDlg.FileName;

                // Запрос имени
                string name = Microsoft.VisualBasic.Interaction.InputBox(
                    "Введите название маскота:", "Имя маскота",
                    Path.GetFileNameWithoutExtension(sourceJarPath));

                if (string.IsNullOrWhiteSpace(name))
                    name = Path.GetFileNameWithoutExtension(sourceJarPath);

                // копирка jar в локальное хранилище (или сохранение ориг пути)
                string storageDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "ShimejiManager", "Mascots");
                Directory.CreateDirectory(storageDir);

                string destJarPath = Path.Combine(storageDir, Path.GetFileName(sourceJarPath));
                File.Copy(sourceJarPath, destJarPath, overwrite: true);

                var mascot = new Mascot // создание обьекта
                {
                    Name = name,
                    JarPath = destJarPath, // или sourceJarPath, если не копировать
                    IsRunning = false
                };

                mascots.Add(mascot);
                // Сохранение происходит автоматически
            }
        }
        private bool IsJavaAvailable() // проверка наличия Java для возможности запуска
        {
            try
            {
                var psi = new ProcessStartInfo("java", "-version")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };
                using var proc = Process.Start(psi);
                proc.WaitForExit(3000);
                return proc.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private void Runbtn_Click(object sender, EventArgs e)
        {

            if (dgvMascots.CurrentRow?.DataBoundItem is Mascot selected)
            {
                if (selected.IsRunning)
                {
                    MessageBox.Show("Маскот уже запущен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!IsJavaAvailable())
                {
                    MessageBox.Show("Java не найдена.Ошибка");
                    return;
                }
                try
                {
                    _processManager.Start(selected);
                    dgvMascots.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка запуска: {ex.Message}");
                }
            }
        }

        private void Stopbtn_Click(object sender, EventArgs e)
        {
            if (dgvMascots.CurrentRow?.DataBoundItem is Mascot selected)
            {
                _processManager.Stop(selected);
                dgvMascots.Refresh();
            }
        }

        private void Deletebtn_Click(object sender, EventArgs e)
        {
            if (dgvMascots.CurrentRow?.DataBoundItem is Mascot selected)
            {
                if (selected.IsRunning)
                {
                    var result = MessageBox.Show("Маскот сейчас запущен. Остановить и удалить?",
                        "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // Останавливаем используя логику кнопки стоп
                        {
                            _processManager.Stop(selected);
                            dgvMascots.Refresh();
                        }
                    }
                    else return;
                }

                // Удаляем jar-файл из хранилища (если вы его копировали туда)
                if (File.Exists(selected.JarPath))
                {
                    try { File.Delete(selected.JarPath); }
                    catch { /* игнорируем ошибку */ }
                }

                mascots.Remove(selected);
                dgvMascots.Refresh();
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var running = mascots.Where(m => m.IsRunning).ToList();
            if (running.Any())
            {
                var dlg = MessageBox.Show($"Запущено маскотов: {running.Count}. Остановить их перед выходом?",
                    "Завершение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dlg == DialogResult.Yes)
                {
                    foreach (var m in running)
                        StopMascot(m);
                }
                else if (dlg == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                // Если "Нет" – оставляем процессы работать, приложение завершится
            }
        }
        private void StopMascot(Mascot mascot)
        {
            if (!mascot.IsRunning || mascot.ProcessId == null)
                return;

            try
            {
                var proc = Process.GetProcessById(mascot.ProcessId.Value);
                if (!proc.HasExited)
                {
                    // Пытаемся закрыть главное окно
                    proc.CloseMainWindow();
                    // Ждём 3 секунды
                    if (!proc.WaitForExit(3000))
                    {
                        // Если не завершился — принудительно
                        proc.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                // Процесс уже мог быть завершён или нет прав доступа
                // Логируем или показываем сообщение, если нужно
                Debug.WriteLine($"Ошибка остановки маскота: {ex.Message}");
            }
            finally
            {
                mascot.IsRunning = false;
                mascot.ProcessId = null;
                // Обновляем таблицу (если привязан BindingList, он уведомит DataGridView)
                dgvMascots.Refresh();
            }
        }


        // подтяжка списка на вторую страницу
        private List<string> GetMascotNames() => mascots.Select(m => m.Name).ToList();
        private void AddNotbtn_Click(object sender, EventArgs e) // кнопка на второй странице
        {
            using var editor = new NotificationEditForm(GetMascotNames());
            if (editor.ShowDialog() == DialogResult.OK)
            {
                _notifications.Add(editor.Notification);
            }
        }

        private void EditNotbtn_Click(object sender, EventArgs e)
        {
            if (dgvNotifications.CurrentRow?.DataBoundItem is ScheduledNotification selected)
            {
                using var editor = new NotificationEditForm(selected, GetMascotNames());
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    // Обновление всех связанных полей произойдёт автоматически через привязку
                    dgvNotifications.Refresh();
                }
            }
        }

        private void DeleteNotbtn_Click(object sender, EventArgs e)
        {
            if (dgvNotifications.CurrentRow?.DataBoundItem is ScheduledNotification selected)
            {
                _notifications.Remove(selected);
            }
        }
        // создаем таймер 
        private void TimerCheck_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            bool anyChange = false;
            foreach (var notif in _notifications.ToList()) // копия для безопасного перебора
            {
                if (notif.Executed || notif.Time > now)
                    continue;

                // Уведомление должно сработать
                ShowNotificationBubble(notif);   // метод показа пузыря (реализуем позже)

                anyChange = true;
                if (notif.Repeat != RepeatMode.None)
                {
                    // Вычисляем следующее время
                    switch (notif.Repeat)
                    {
                        case RepeatMode.Daily:
                            notif.Time = notif.Time.AddDays(1);
                            break;
                        case RepeatMode.Weekly:
                            notif.Time = notif.Time.AddDays(7);
                            break;
                    }
                    notif.Executed = false; // сброс для повторного срабатывания
                }
                else
                {
                    notif.Executed = true; // однократное – выполнено
                }
            }
            if (anyChange)
                dgvNotifications.Refresh();
        }
        private void ShowNotificationBubble(ScheduledNotification notif)// заглушка для пузыря пока что
        {
            // Временная отладка – потом заменим на графический пузырь
            MessageBox.Show(notif.Message, $"Уведомление для {notif.TargetMascotName}",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
