using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimejiPlan124325.Services
{
    public static class Logger
    {
        private static readonly string LogDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ShimejiManager", "logs");

        private static readonly string LogFile = Path.Combine(LogDir, $"app_{DateTime.Now:yyyy-MM-dd}.log");

        private static readonly object _lock = new object();

        public static void Write(string level, string message)// запись соо в лог файл
        {
            lock (_lock)
            {
                try
                {
                    Directory.CreateDirectory(LogDir);
                    string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}";
                    File.AppendAllText(LogFile, entry);
                }
                catch
                {
                    // ошибка логирования не логируется 
                }
            }
        }

        public static void Info(string message) => Write("INFO", message);
        public static void Error(string message) => Write("ERROR", message);
        public static void Error(string message, Exception ex) => Write("ERROR", $"{message} | Exception: {ex}");
    }
}
