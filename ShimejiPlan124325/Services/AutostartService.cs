using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimejiPlan124325.Services
{
    public static class AutostartService
    {
        
        private static string StartupFolder =>
            Environment.GetFolderPath(Environment.SpecialFolder.Startup);// Путь к папке автозагрузки текущего пользователя

        
        private static string ShortcutName => "ShimejiManager.lnk";// Имя ярлыка (обычно совпадает с названием приложения)

        
        private static string ShortcutPath =>
            Path.Combine(StartupFolder, ShortcutName);// Полный путь к ярлыку

        public static void SetAutostart(bool enable)
        {
            if (enable)
            {
                CreateShortcut();
            }
            else
            {
                RemoveShortcut();
            }
        }
        public static bool IsAutostartEnabled()
        {
            return File.Exists(ShortcutPath);
        }

        private static void CreateShortcut()
        {
            try
            {
                RemoveShortcut(); // Удаляем старый ярлык, если есть
                string appPath = Application.ExecutablePath;

                Type? shellType = Type.GetTypeFromProgID("WScript.Shell");
                if (shellType == null)
                {
                    Logger.Error("Не удалось получить тип WScript.Shell. COM-объект не зарегистрирован.");
                    return;
                }

                object? shellObj = Activator.CreateInstance(shellType);
                if (shellObj == null)
                {
                    Logger.Error("Не удалось создать экземпляр WScript.Shell.");
                    return;
                }
                dynamic shell = shellObj;
                dynamic shortcut = shell.CreateShortcut(ShortcutPath);
                shortcut.TargetPath = appPath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(appPath);
                shortcut.Description = "Shimeji Manager";
                shortcut.Save();
            }
            catch (Exception ex)
            {
                Logger.Error($"Не удалось создать ярлык автозагрузки: {ex.Message}");
            }
        }

        private static void RemoveShortcut()
        {
            try
            {
                if (File.Exists(ShortcutPath))
                {
                    File.Delete(ShortcutPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Не удалось удалить ярлык автозагрузки: {ex.Message}");
            }
        }
    }
}
