using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShimejiPlan124325.Models
{
    public enum RepeatMode
    {
        None,       // одиночное
        Daily,      // ежедневно
        Weekly      // раз в неделю
    }
    public class ScheduledNotification : INotifyPropertyChanged
    {

        private DateTime _time;
        private string _message;
        private string _targetMascotName; // "Любой активный" или конкретное имя
        private RepeatMode _repeat;
        private bool _executed;

        public DateTime Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public string TargetMascotName
        {
            get => _targetMascotName;
            set { _targetMascotName = value; OnPropertyChanged(); }
        }

        public RepeatMode Repeat
        {
            get => _repeat;
            set { _repeat = value; OnPropertyChanged(); }
        }

        public bool Executed
        {
            get => _executed;
            set { _executed = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public string RepeatDisplay => Repeat switch
        {
            RepeatMode.None => "Один раз",
            RepeatMode.Daily => "Ежедневно",
            RepeatMode.Weekly => "Еженедельно",
            _ => ""
        };

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    public class NotificationStorage
    {
        private static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ShimejiManager", "notifications.json");

        public static List<ScheduledNotification> Load()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<ScheduledNotification>>(json) ?? new List<ScheduledNotification>();
            }
            return new List<ScheduledNotification>();
        }

        public static void Save(IEnumerable<ScheduledNotification> notifications)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            var json = JsonSerializer.Serialize(notifications, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
