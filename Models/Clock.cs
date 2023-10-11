namespace MCUNetwork.Models
{
    public class Clock
    {
        public event Action<int>? OnNextTick;

        public int ElapsedTicks { get; private set; }
        public int ElapsedTime { get; private set; }
        public int Delay { get; set; }
        public int Delta { get; set; }
        public bool IsRunning { get; private set; } = false;


        public async Task Run(int duration)
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("You have already started a clock");
            }

            ElapsedTicks = 0;
            ElapsedTime = 0;
            IsRunning = true;

            while (IsRunning)
            {
                if (ElapsedTime < duration)
                {
                    ElapsedTicks++; 
                    ElapsedTime = ElapsedTicks * Delta;

                    OnNextTick?.Invoke(ElapsedTicks);
                    await Task.Delay(Delay);
                } else
                {
                    IsRunning = false;
                }
            }
        }

        public Task Await(int ticks = 1)
        {
            int finishAt = ElapsedTicks + ticks * Delta;
            var source = new TaskCompletionSource<int>();

            if (IsRunning)
            {
                Action<int> handler = null!;

                handler = elapsedTicks => 
                {
                    if (elapsedTicks >= finishAt)
                    {
                        OnNextTick -= handler;
                        source.SetResult(elapsedTicks);
                    }
                };

                OnNextTick += handler;
            }

            return source.Task;
        }

        public void RunEachTicks(Action<int> action, int eachTicks) => ScheduleAction(action, eachTicks);

        public void Stop()
        {
            IsRunning = false;            
            OnNextTick = null;
        }

        private void ScheduleAction(Action<int> action, int eachTicks)
        {
            Action<int> handler = null!;

            long last = ElapsedTicks;

            handler = elapsedTicks =>
            {
                if (elapsedTicks - eachTicks >= last)
                {
                    action.Invoke(elapsedTicks);
                    last = elapsedTicks;
                }
            };

            OnNextTick += handler;
        }
    }
}
