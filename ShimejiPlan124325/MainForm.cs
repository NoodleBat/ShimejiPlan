using ShimejiPlan124325.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace ShimejiPlan124325
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        BindingList<Mascot> mascots = new BindingList<Mascot>(); // для показа в гриде
        private readonly MascotProcessManager _processManager = new MascotProcessManager(); // показ с настройками

        BindingList<ScheduledNotification> notifications = new BindingList<ScheduledNotification>(); // показ уведомлений
        private BindingList<ScheduledNotification> _notifications; //еще к уведам
        private BindingList<TaskItem> _tasks; //поле для задач

        public MainForm()
        {
            InitializeComponent();

            mascots = new BindingList<Mascot>(MascotStorage.Load()); // Загружаем данные
            mascots.ListChanged += (s, e) => MascotStorage.Save(mascots);

            _notifications = new BindingList<ScheduledNotification>(NotificationStorage.Load());
            _notifications.ListChanged += (s, e) => NotificationStorage.Save(_notifications);

            ConfigureMascotsGrid();// Настраиваем таблицы и привязки
            ConfigureNotificationsGrid();

            this.notifyIcon.Icon = System.Drawing.SystemIcons.Information;// иконка для уведомления


            _tasks = new BindingList<TaskItem>(TaskStorage.Load());// Загружаем задачи
            _tasks.ListChanged += (s, e) => TaskStorage.Save(_tasks);
            ConfigureTasksGrid();

            // Запускаем таймер только после того, как всё готово
            timerCheck.Enabled = true;  // или timerCheck.Start();

            Logger.Info("Приложение Shimeji Manager запущено.");
            this.timerCheck.Tick += new System.EventHandler(this.TimerCheck_Tick);

        }
        private void ConfigureMascotsGrid()// запуск DataGridView на первой странице с шимеджи
        {
            dgvMascots.AutoGenerateColumns = false;
            dgvMascots.DataSource = mascots;

            // Добавить колонки (можно оставить как у вас, но лучше вынести в отдельный метод)
            dgvMascots.Columns.Add(new DataGridViewTextBoxColumn // поле имя
            {
                Name = "colName",
                HeaderText = "Имя",
                DataPropertyName = "Name"
            });
            dgvMascots.Columns.Add(new DataGridViewTextBoxColumn// поле путь
            {
                Name = "colPathe",
                HeaderText = "Путь",
                DataPropertyName = "JarPath"
            });
            dgvMascots.Columns.Add(new DataGridViewTextBoxColumn // поле статус
            {
                Name = "colStatus",
                HeaderText = "Статус",
                DataPropertyName = "Status"
            });
            dgvMascots.Columns.Add(new DataGridViewTextBoxColumn // скрытое поле для хранения ProcessId
            {
                Name = "colProcessId",
                HeaderText = "Process ID",
                DataPropertyName = "ProcessId",
                Visible = false
            });

            mascots = new BindingList<Mascot>(MascotStorage.Load());
            mascots.ListChanged += (s, e) => MascotStorage.Save(mascots);// автосохранение
            dgvMascots.DataSource = mascots;

            dgvMascots.AutoGenerateColumns = false; // отключение автогенерации столбов 
            dgvMascots.DataSource = mascots;// привязка грида

        }
        private void ConfigureNotificationsGrid()
        {
            dgvNotifications.AutoGenerateColumns = false;
            dgvNotifications.DataSource = _notifications;

            dgvNotifications.Columns.Add(new DataGridViewTextBoxColumn// поле время
            {
                Name = "colTime",
                HeaderText = "Время",
                DataPropertyName = "Time",
                DefaultCellStyle = { Format = "dd.MM.yyyy HH:mm" }// формат времени
            });
            // ... остальные колонки, ОБЯЗАТЕЛЬНО проверьте имена свойств:
            // Target -> TargetMascotName, Repeat -> Repeat (если в модели Repeat)
            // Например:
            dgvNotifications.Columns.Add(new DataGridViewTextBoxColumn// поле персонаж
            {
                Name = "colTarget",
                HeaderText = "Маскот",
                DataPropertyName = "TargetMascotName"
            });
            dgvNotifications.Columns.Add(new DataGridViewTextBoxColumn// поле сообщение
            {
                Name = "colMessage",
                HeaderText = "Сообщение",
                DataPropertyName = "Message"
            });
            dgvNotifications.Columns.Add(new DataGridViewTextBoxColumn// поле повтор
            {
                Name = "colRepeat",
                HeaderText = "Повтор",
                DataPropertyName = "Repeat"
            });

            // настройка  DataGridView на второй странице с задачами

            dgvNotifications.DataSource = notifications;// привязка второй страницы
            _notifications = new BindingList<ScheduledNotification>(NotificationStorage.Load());
            _notifications.ListChanged += (s, e) => NotificationStorage.Save(_notifications);//автосохр
            dgvNotifications.DataSource = _notifications;
            dgvNotifications.AutoGenerateColumns = false; // минус автогенерация

        }
        // проверка наличия Java для возможности запуска
        private bool IsJavaAvailable
        {
            get
            {
                try
                {
                    var psi = new ProcessStartInfo("java", "-version")
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true
                    };

                    Process? proc = Process.Start(psi);
                    if (proc == null)
                        return false;

                    using (proc)
                    {
                        proc.WaitForExit(3000);
                        return proc.ExitCode == 0;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        // первая страница кнопки
        private void Addbtn_Click(object sender, EventArgs e)// кнопка добваления
        {
            using var openDlg = new OpenFileDialog();
            openDlg.Filter = "Jar-файлы (*.jar)|*.jar|Все файлы (*.*)|*.*";
            openDlg.Title = "Выберите исполняемый Jar Shimeji";
            if (openDlg.ShowDialog() != DialogResult.OK)
                return;

            string sourceJarPath = openDlg.FileName;

            string name = Microsoft.VisualBasic.Interaction.InputBox(// Запрашиваем имя маскота
                "Введите название маскота:", "Имя маскота",
                Path.GetFileNameWithoutExtension(sourceJarPath));
            if (string.IsNullOrWhiteSpace(name))
                name = Path.GetFileNameWithoutExtension(sourceJarPath);

            string mascotStorageDir = Path.Combine(// Папка-хранилище для этого маскота
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ShimejiManager", "Mascots", name);
            Directory.CreateDirectory(mascotStorageDir);

            string destJarPath = Path.Combine(mascotStorageDir, Path.GetFileName(sourceJarPath));// Копируем сам .jar-файл
            File.Copy(sourceJarPath, destJarPath, overwrite: true);

            string? sourceDir = Path.GetDirectoryName(sourceJarPath);// Копируем все папки, лежащие рядом с исходным .jar
            if (sourceDir != null)
            {
                foreach (var dir in Directory.GetDirectories(sourceDir))
                {
                    string dirName = Path.GetFileName(dir);
                    string destDir = Path.Combine(mascotStorageDir, dirName);
                    CopyDirectoryRecursive(dir, destDir);
                }
            }

            var mascot = new Mascot// Создаём объект маскота и добавляем в список
            {
                Name = name,
                JarPath = destJarPath,
                IsRunning = false
            };
            mascots.Add(mascot);
        }

        private void Runbtn_Click(object sender, EventArgs e) // кнопка запустить перс
        {

            if (dgvMascots.CurrentRow?.DataBoundItem is Mascot selected)
            {
                if (selected.IsRunning)
                {
                    MessageBox.Show("Маскот уже запущен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!IsJavaAvailable)
                {
                    Logger.Error("Java не найдена или не доступна.");
                    MessageBox.Show("Java не найдена. Пожалуйста, установите JRE и пропишите путь в системных переменных.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    Logger.Info($"Запуск маскота '{selected.Name}' из '{selected.JarPath}'");
                    _processManager.Start(selected);
                    dgvMascots.Refresh();
                    Logger.Info($"Маскот '{selected.Name}' успешно запущен, PID: {selected.ProcessId}");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Ошибка запуска маскота '{selected.Name}'", ex);
                    MessageBox.Show($"Ошибка запуска: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Stopbtn_Click(object sender, EventArgs e)// кнопка стоп перс
        {
            // Проверка, выбрана ли строка
            if (dgvMascots.CurrentRow == null)
            {
                MessageBox.Show("Выберите маскота в таблице.", "Остановка",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!(dgvMascots.CurrentRow.DataBoundItem is Mascot selected))
                return;

            // Если уже не запущен – просто сбрасываем состояние и выходим
            if (!selected.IsRunning || selected.ProcessId == null)
            {
                selected.IsRunning = false;
                selected.ProcessId = null;
                dgvMascots.Refresh();
                return;
            }

            // Сохраняем PID до вызова Stop (Stop обнулит selected.ProcessId)
            int pid = selected.ProcessId.Value;

            try
            {
                // 1. Мягкая остановка через менеджер
                _processManager.Stop(selected);
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в менеджере остановки: {ex.Message}");
            }

            // 2. Принудительное завершение процесса, если он ещё жив
            try
            {
                Process proc = Process.GetProcessById(pid);
                if (!proc.HasExited)
                {
                    proc.Kill();
                    proc.WaitForExit(2000);
                    Logger.Info($"Процесс {pid} принудительно завершён.");
                }
            }
            catch (ArgumentException)
            {
                // Процесс с таким Id уже не существует – это нормально
                Logger.Info($"Процесс {pid} уже завершён.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при принудительной остановке процесса {pid}: {ex.Message}");
            }

            // 3. Сброс состояния в любом случае
            selected.IsRunning = false;
            selected.ProcessId = null;
            dgvMascots.Refresh();
        }

        private void Deletebtn_Click(object sender, EventArgs e)// кнопка удалить перс
        {
            if (dgvMascots.CurrentRow?.DataBoundItem is Mascot selected)
            {
                if (selected.IsRunning)
                {
                    var result = MessageBox.Show("Маскот сейчас запущен. Остановить и удалить?",
                        "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)// Останавливаем используя логику кнопки стоп
                    {
                        {
                            _processManager.Stop(selected);
                            dgvMascots.Refresh();
                        }
                    }
                    else return;
                }
                if (File.Exists(selected.JarPath))// Удаляем jar-файл из хранилища 
                {
                    try { File.Delete(selected.JarPath); }
                    catch { /* игнорируем ошибку */ }
                }
                mascots.Remove(selected);
                dgvMascots.Refresh();
            }
        }
        private void StopMascot(Mascot mascot) //кнопка стоп перс
        {
            if (!mascot.IsRunning || mascot.ProcessId == null)
                return;

            try
            {
                var proc = Process.GetProcessById(mascot.ProcessId.Value);
                if (!proc.HasExited)
                {
                    proc.CloseMainWindow();// попытка закрыть главное окно
                    if (!proc.WaitForExit(3000))// ждём 3 секунды
                    {
                        proc.Kill();// если что то принудительно
                    }
                }
            }
            catch (Exception ex)// если процесс уже завершён или нет прав доступа
            {
                Debug.WriteLine($"Ошибка остановки маскота: {ex.Message}");
            }
            finally
            {
                mascot.IsRunning = false;
                mascot.ProcessId = null;
                dgvMascots.Refresh();// Обновляем таблицу (если привязан BindingList, он уведомит DataGridView)
            }
        }
        private void Settingsbtn_Click(object sender, EventArgs e)// кнопка настроек
        {
            using var settingsForm = new SettingsForm();
            settingsForm.ShowDialog(this);
        }

        // кнопки второй формы
        public void ShowNotificationBubble(ScheduledNotification notif)
        {
            RECT? mascotRect = null;

            if (notif.TargetMascotName == "Любой активный")
            {
                var result = ShimejiWindowFinder.FindAnyActiveWindow();   // ← сервис
                if (result != null) mascotRect = result.Value.rect;
            }
            else
            {
                Mascot? target = mascots.FirstOrDefault(m => m.Name == notif.TargetMascotName);
                if (target != null)
                {
                    var result = ShimejiWindowFinder.FindWindowByMascot(target);  // ← сервис
                    if (result != null) mascotRect = result.Value.rect;
                }
            }

            if (mascotRect != null)
            {
                var bubble = new BubbleNotificationForm(notif.Message);
                bubble.PositionNearMascot(mascotRect.Value);
                bubble.Show();
            }
            else
            {
                notifyIcon.ShowBalloonTip(5000, "Shimeji Manager", notif.Message, ToolTipIcon.Info);
            }
        }
        private void EditNotbtn_Click(object sender, EventArgs e)// кнопка изменение напоминаний
        {
            if (dgvNotifications.CurrentRow?.DataBoundItem is ScheduledNotification selected)
            {
                using var editor = new NotificationEditForm(selected, GetMascotNames());
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    dgvNotifications.Refresh();// обновление автомат
                }
            }
        }
        private void DeleteNotbtn_Click(object sender, EventArgs e)// кнопка удаления напоминаний
        {
            if (dgvNotifications.CurrentRow?.DataBoundItem is ScheduledNotification selected)
            {
                _notifications.Remove(selected);
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
        // кнопка на третьей странице
        private void btnOpenFocus_Click(object sender, EventArgs e)
        {
            var settings = FocusSettingsStorage.Load();
            var focusForm = new FocusForm(settings);
            focusForm.FormClosed += (s, args) =>
            {
                // Главное окно может быть скрыто, показываем снова, если нужно
                this.Show();
                this.WindowState = FormWindowState.Normal;
            };

            // Опционально сворачиваем главное окно в трей
            this.Hide();
            focusForm.Show();
        }

        // прочие вспомогательные и в основном общие штуки

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
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var running = mascots.Where(m => m.IsRunning && m.ProcessId != null).ToList();
            if (running.Any())
            {
                var dlg = MessageBox.Show($"Запущено маскотов: {running.Count}. Остановить их перед выходом?",
                    "Завершение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dlg == DialogResult.Yes)
                {
                    foreach (var m in running)
                    {
                        try
                        {
                            _processManager.Stop(m);
                            // Надёжно убиваем, если ещё жив
                            if (m.ProcessId != null)
                            {
                                var proc = Process.GetProcessById(m.ProcessId.Value);
                                if (!proc.HasExited) proc.Kill();
                            }
                        }
                        catch { /* игнорируем, всё равно выходим */ }
                        finally
                        {
                            m.IsRunning = false;
                            m.ProcessId = null;
                        }
                    }
                }
                else if (dlg == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private static void CopyDirectoryRecursive(string sourceDir, string destDir)// Рекурсивно копирует директорию со всем содержимым.
        {
            Directory.CreateDirectory(destDir);// Создаём целевую папку, если её нет

            foreach (var file in Directory.GetFiles(sourceDir))// Копируем все файлы
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, overwrite: true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))// Рекурсивно обрабатываем подпапки
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
                CopyDirectoryRecursive(dir, destSubDir);
            }
        }
        private void ConfigureTasksGrid() // метод настройки таблицы задач
        {
            dgvTasks.AutoGenerateColumns = false;
            dgvTasks.DataSource = _tasks;

            dgvTasks.Columns.Clear();// Очищаем колонки, если они уже были добавлены

            var checkColumn = new DataGridViewCheckBoxColumn// Колонка с чекбоксом
            {
                Name = "colCompleted",
                HeaderText = "",
                DataPropertyName = "IsCompleted",
                Width = 30,
                TrueValue = true,
                FalseValue = false
            };
            dgvTasks.Columns.Add(checkColumn);

            var textColumn = new DataGridViewTextBoxColumn// Текстовая колонка
            {
                Name = "colTaskText",
                HeaderText = "Описание задачи",
                DataPropertyName = "Text",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            dgvTasks.Columns.Add(textColumn);
            dgvTasks.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex >= 0 && dgvTasks.Columns[e.ColumnIndex].Name == "colCompleted")
                {
                    TaskStorage.Save(_tasks); // сохраняем изменения
                }
            };
        }

        private void btnAddTask_Click(object sender, EventArgs e)//кнопка добаувить задачи
        {
            string taskText = Microsoft.VisualBasic.Interaction.InputBox(
        "Введите текст задачи:", "Новая задача", "");
            if (!string.IsNullOrWhiteSpace(taskText))
            {
                _tasks.Add(new TaskItem { Text = taskText.Trim(), IsCompleted = false });
            }
        }

        private void btnDeleteTask_Click(object sender, EventArgs e)// кнопка удалить задачи
        {
            if (dgvTasks.CurrentRow?.DataBoundItem is TaskItem selected)
            {
                _tasks.Remove(selected);
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления.", "Удаление",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void tabPage2_Click(object sender, EventArgs e)//случайно, потом уберу когла нибудь
        {

        }
    }
}
