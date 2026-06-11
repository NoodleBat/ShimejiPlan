using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimejiPlan124325.Models
{
    public class FocusSession
    {
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsWork { get; set; }
        public string? TaskTitle { get; set; }
    }
}
