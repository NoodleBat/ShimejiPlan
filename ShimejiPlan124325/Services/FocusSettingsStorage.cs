using ShimejiPlan124325.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShimejiPlan124325.Services
{
    public static class FocusStatsStorage
    {
        private static string BaseDir => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ShimejiManager");

        public static FocusStats LoadStats()
        {
            string path = Path.Combine(BaseDir, "focus_stats.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<FocusStats>(json) ?? new FocusStats();
            }
            return new FocusStats();
        }

        public static void SaveStats(FocusStats stats)
        {
            Directory.CreateDirectory(BaseDir);
            string path = Path.Combine(BaseDir, "focus_stats.json");
            var json = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static List<FocusTask> LoadTasks()
        {
            string path = Path.Combine(BaseDir, "focus_tasks.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<FocusTask>>(json) ?? new List<FocusTask>();
            }
            return new List<FocusTask>();
        }

        public static void SaveTasks(IEnumerable<FocusTask> tasks)
        {
            Directory.CreateDirectory(BaseDir);
            string path = Path.Combine(BaseDir, "focus_tasks.json");
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}
