using ShimejiPlan124325.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ShimejiPlan124325.Services
{
    public enum FocusState { Idle, Working, OnBreak }
    public class FocusTimer
    {
        public TimeSpan Remaining { get; private set; }
        public FocusState State { get; private set; } = FocusState.Idle;

        private System.Timers.Timer? _timer;
        private FocusSettings _settings;

        public event Action<TimeSpan, FocusState> Tick;
        public event Action<FocusState> StateChanged;

        public FocusTimer(FocusSettings settings)
        {
            _settings = settings;
            Tick = (time, state) => { };
            StateChanged = (state) => { };
        }

        public void StartWork()
        {
            Remaining = TimeSpan.FromMinutes(_settings.WorkDurationMin);
            State = FocusState.Working;
            StateChanged?.Invoke(State);
            StartCountdown();
        }

        public void StartBreak()
        {
            Remaining = TimeSpan.FromMinutes(_settings.BreakDurationMin);
            State = FocusState.OnBreak;
            StateChanged?.Invoke(State);
            StartCountdown();
        }

        public void Pause() => _timer?.Stop();
        public void Resume() => _timer?.Start();

        public void Reset()
        {
            _timer?.Stop();
            State = FocusState.Idle;
            StateChanged?.Invoke(State);
            Remaining = TimeSpan.Zero;
        }

        private void StartCountdown()
        {
            _timer?.Stop();
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            var timer = _timer;      // захватываем текущее значение
            Remaining = Remaining.Subtract(TimeSpan.FromSeconds(1));
            Tick?.Invoke(Remaining, State);

            if (Remaining <= TimeSpan.Zero)
            {
                timer?.Stop();       // используем локальную копию для более вероятного избежания ошибок
                if (State == FocusState.Working)
                    StartBreak();
                else
                    Reset();
            }
        }
    }
}
