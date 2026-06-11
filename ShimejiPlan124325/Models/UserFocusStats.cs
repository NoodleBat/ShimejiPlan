using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimejiPlan124325.Models
{
    public class UserFocusStats
    {
        public int TotalSessions { get; set; }
        public TimeSpan TotalWorkTime { get; set; }
        public TimeSpan TotalBreakTime { get; set; }
        public int CurrentLevel { get; set; }
        public TimeSpan TimeToNextLevel { get; set; }
        public List<string> Achievements { get; set; } = new();

        public static UserFocusStats Calculate(List<FocusSession> sessions)
        {
            var stats = new UserFocusStats();
            TimeSpan totalWork = TimeSpan.Zero;
            TimeSpan totalBreak = TimeSpan.Zero;

            foreach (var s in sessions)
            {
                if (s.IsWork) totalWork += s.Duration;
                else totalBreak += s.Duration;
            }

            stats.TotalSessions = sessions.Count;
            stats.TotalWorkTime = totalWork;
            stats.TotalBreakTime = totalBreak;

            int level = (int)(totalWork.TotalHours / 10) + 1;
            stats.CurrentLevel = level;
            double hoursForNext = (level) * 10 - totalWork.TotalHours;
            stats.TimeToNextLevel = TimeSpan.FromHours(hoursForNext > 0 ? hoursForNext : 0);

            var achievements = new List<string>();
            if (sessions.Count >= 1)
                achievements.Add("Первая сессия");
            if (totalWork.TotalHours >= 1)
                achievements.Add("1 час работы");
            if (totalWork.TotalHours >= 10)
                achievements.Add("10 часов работы");
            stats.Achievements = achievements;

            return stats;
        }
    }
}
