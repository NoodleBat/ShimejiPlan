using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShimejiPlan124325.Models
{
    public class FocusTask : INotifyPropertyChanged
    {
        private string _text;
        private bool _isDone;

        public string Text
        {
            get => _text;
            set { _text = value; OnPropertyChanged(); }
        }

        public bool IsDone
        {
            get => _isDone;
            set { _isDone = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
