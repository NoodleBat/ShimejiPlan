using ShimejiPlan124325.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShimejiPlan124325.Services
{
    public static class FocusSessionStorage
    {
        private static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ShimejiManager", "focus_sessions.json");

        public static List<FocusSession> Load()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<FocusSession>>(json) ?? new List<FocusSession>();
            }
            return new List<FocusSession>();
        }

        public static void Save(IEnumerable<FocusSession> sessions)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(sessions, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
