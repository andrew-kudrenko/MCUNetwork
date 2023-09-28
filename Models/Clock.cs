﻿namespace MCUNetwork.Models
{
    public class Clock
    {
        public delegate void ScheduledActionHandler();
        public delegate void NextTickHandler(long elapsedTime);

        public event NextTickHandler? OnNextTick;
        public event NextTickHandler? OnEnd;

        public int Delay { get; set; } = 1_000;

        private bool _isRunning = false;
        private long _elapsedTime = 0;

        public async Task Run(long duration, int delta)
        {
            if (_isRunning)
            {
                throw new InvalidOperationException("You have already started a clock");
            }

            _isRunning = true;
            _elapsedTime = 0;

            while (_isRunning)
            {
                if (_elapsedTime < duration)
                {
                    await Task.Delay(Delay);
                    OnNextTick?.Invoke(_elapsedTime);
                    _elapsedTime += delta;
                } else
                {
                    _isRunning = false;
                }

            }

            OnEnd?.Invoke(_elapsedTime);
        }

        public void Stop()
        {
            _isRunning = false;
            
            OnEnd?.Invoke(_elapsedTime);
            OnNextTick = null!;
        }

        public void ExecuteUntil(long each, long until, ScheduledActionHandler action) => ScheduleAction(each, until, action);

        public void ExecuteAfter(long ticks, ScheduledActionHandler action) => ScheduleAction(1, ticks + _elapsedTime, action);

        private void ScheduleAction(long each, long until, ScheduledActionHandler action)
        {
            NextTickHandler handler = null!;

            long last = _elapsedTime;

            handler = elapsed =>
            {
                if (elapsed - each >= last)
                {
                    action.Invoke();

                    if (elapsed >= until)
                    {
                        OnNextTick -= handler;
                    }
                }
            };

            OnNextTick += handler;
        }
    }
}