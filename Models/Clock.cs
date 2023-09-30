namespace MCUNetwork.Models
{
    public class Clock
    {
        public delegate void ScheduledActionHandler();
        public delegate void NextTickHandler(long elapsedTime);
        public delegate void StartHandler();

        public event NextTickHandler? OnNextTick;
        public event NextTickHandler? OnEnd;
        public event StartHandler? OnStart;

        public int Delay { get; set; } = 1_000;
        public bool IsRunning { get; private set; } = false;

        private long _elapsedTime = 0;

        public async Task Run(long duration, int delta)
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("You have already started a clock");
            }

            IsRunning = true;
            _elapsedTime = 0;

            OnStart?.Invoke();

            while (IsRunning)
            {
                if (_elapsedTime < duration)
                {
                    await Task.Delay(Delay);
                    OnNextTick?.Invoke(_elapsedTime);
                    _elapsedTime += delta;
                } else
                {
                    IsRunning = false;
                }

            }

            OnEnd?.Invoke(_elapsedTime);
        }

        public void Stop()
        {
            IsRunning = false;
            
            OnEnd?.Invoke(_elapsedTime);
            OnNextTick = null!;
        }

        public void ExecuteEach(ScheduledActionHandler action, int each) => ScheduleAction(action, each);

        public void ExecuteUntil(ScheduledActionHandler action, int each, long until) => ScheduleAction(action, each, until);

        public void ExecuteAfter(ScheduledActionHandler action, int ticks) => ScheduleAction(action, 1, ticks + _elapsedTime);

        private void ScheduleAction(ScheduledActionHandler action, long each, long until)
        {
            NextTickHandler handler = null!;

            long last = _elapsedTime;

            handler = elapsed =>
            {
                if (elapsed - each >= last)
                {
                    action.Invoke();
                    last = elapsed;

                    if (elapsed >= until)
                    {
                        OnNextTick -= handler;
                    }
                }
            };

            OnNextTick += handler;
        }

        private void ScheduleAction(ScheduledActionHandler action, long each)
        {
            NextTickHandler handler = null!;

            long last = _elapsedTime;

            handler = elapsed =>
            {
                if (elapsed - each >= last)
                {
                    action.Invoke();
                    last = elapsed;
                }
            };

            OnNextTick += handler;
        }
    }
}
