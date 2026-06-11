using ShimejiPlan124325.Models;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ShimejiPlan124325.Services
{
    public static class ShimejiWindowFinder
    {
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
            public int Width => Right - Left;
            public int Height => Bottom - Top;
        }

        /// <summary>
        /// Известные классы окон Shimeji для разных версий.
        /// </summary>
        public static readonly HashSet<string> KnownShimejiClasses = new HashSet<string>
        {
            "SunAwtFrame",
            "ShimejiWindow",
            "LWWindowClass",
            "SunAwtDialog",
            "ShimejiFrame"
        };

        /// <summary>
        /// Находит окно указанного маскота (по его ProcessId).
        /// Возвращает дескриптор и координаты, либо null, если маскот не запущен или окно не найдено.
        /// </summary>
        public static (IntPtr hWnd, RECT rect)? FindWindowByMascot(Mascot mascot)
        {
            if (!mascot.IsRunning || mascot.ProcessId == null)
                return null;

            IntPtr hWnd = FindMainWindowOfProcess(mascot.ProcessId.Value);
            if (hWnd == IntPtr.Zero)
                return null;

            if (!GetWindowRect(hWnd, out RECT rect))
                return null;

            return (hWnd, rect);
        }

        /// <summary>
        /// Находит любое видимое окно Shimeji (первое попавшееся).
        /// Возвращает дескриптор и координаты, либо null, если ни одного маскота нет на экране.
        /// </summary>
        public static (IntPtr hWnd, RECT rect)? FindAnyActiveWindow()
        {
            IntPtr hWnd = FindAnyShimejiWindow();
            if (hWnd == IntPtr.Zero)
                return null;

            if (!GetWindowRect(hWnd, out RECT rect))
                return null;

            return (hWnd, rect);
        }

        /// <summary>
        /// Главный метод поиска окна для конкретного процесса.
        /// Сначала пробует MainWindowHandle, затем перебор окон процесса.
        /// </summary>
        public static IntPtr FindMainWindowOfProcess(int processId)
        {
            try
            {
                var proc = Process.GetProcessById(processId);
                IntPtr mainHandle = proc.MainWindowHandle;
                if (mainHandle != IntPtr.Zero && IsWindowVisible(mainHandle))
                    return mainHandle;

                // MainWindowHandle может быть нулевым или невидимым – перебираем все окна процесса
                return FindWindowByProcessId(processId);
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Перебирает все окна, принадлежащие процессу, и возвращает наиболее подходящее.
        /// Критерии: видимое, размер > 50x50 пикселей (чтобы отсеять служебные окна).
        /// Приоритет – окно с заголовком или известным классом.
        /// </summary>
        public static IntPtr FindWindowByProcessId(int processId)
        {
            IntPtr bestWindow = IntPtr.Zero;
            int bestScore = -1;

            EnumWindows((hWnd, lParam) =>
            {
                GetWindowThreadProcessId(hWnd, out uint pid);
                if (pid != processId)
                    return true;

                if (!IsWindowVisible(hWnd))
                    return true;

                // Получаем размеры окна
                if (!GetWindowRect(hWnd, out RECT rect))
                    return true;

                int width = rect.Width;
                int height = rect.Height;

                // Игнорируем слишком маленькие окна (например, тултипы, меню)
                if (width < 50 || height < 50)
                    return true;

                // Считаем «очки» для окна
                int score = 0;

                // Окно с заголовком (текстом) получает больше очков
                int textLength = GetWindowTextLength(hWnd);
                if (textLength > 0)
                    score += 100;

                // Окно с известным классом Shimeji получает больше очков
                StringBuilder className = new StringBuilder(256);
                GetClassName(hWnd, className, className.Capacity);
                if (KnownShimejiClasses.Contains(className.ToString()))
                    score += 200;

                // Чем больше окно, тем лучше (но не переусердствуем)
                score += Math.Min(width / 10, 50) + Math.Min(height / 10, 50);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestWindow = hWnd;
                }

                return true;
            }, IntPtr.Zero);

            return bestWindow;
        }

        /// <summary>
        /// Ищет любое видимое окно Shimeji без привязки к конкретному процессу.
        /// Сначала проверяет окна с известными классами, затем все процессы Java.
        /// </summary>
        public static IntPtr FindAnyShimejiWindow()
        {
            IntPtr foundWindow = IntPtr.Zero;

            // 1. Быстрый поиск по известным классам (для обратной совместимости)
            foreach (var className in KnownShimejiClasses)
            {
                foundWindow = FindWindowByClassName(className);
                if (foundWindow != IntPtr.Zero)
                    return foundWindow;
            }

            // 2. Ищем среди всех java-процессов
            Process[] javaProcesses = Process.GetProcessesByName("java");
            foreach (var proc in javaProcesses)
            {
                try
                {
                    IntPtr hWnd = FindMainWindowOfProcess(proc.Id);
                    if (hWnd != IntPtr.Zero)
                        return hWnd;
                }
                catch
                {
                    // Пропускаем недоступные процессы
                }
            }

            return IntPtr.Zero;
        }

        public static IntPtr FindWindowByClassName(string className)
        {
            IntPtr foundWindow = IntPtr.Zero;
            EnumWindows((hWnd, lParam) =>
            {
                if (!IsWindowVisible(hWnd))
                    return true;

                StringBuilder currentClass = new StringBuilder(256);
                GetClassName(hWnd, currentClass, currentClass.Capacity);
                if (currentClass.ToString() == className)
                {
                    foundWindow = hWnd;
                    return false; // прекращаем
                }
                return true;
            }, IntPtr.Zero);

            return foundWindow;
        }
    }
}
