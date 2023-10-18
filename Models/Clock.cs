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

        public Task WaitTicks(int ticks = 1)
        {
            int finishAt = ElapsedTicks + ticks;
            var source = new TaskCompletionSource<int>();

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

            return source.Task;
        }

        public void Stop()
        {
            IsRunning = false;            
            OnNextTick = null;
        }

        public void OnTick(Action<int> action, int ticks = 1)
        {
            int now = ElapsedTicks;

            OnNextTick += elapsedTicks =>
            {
                if (elapsedTicks - ticks >= now)
                {
                    action(elapsedTicks);
                    now = elapsedTicks;
                }
            };
        }

        public void OnTime(Action<int> action, int time = 1)
        {
            int now = ElapsedTime;

            OnNextTick += elapsedTicks =>
            {
                if (ElapsedTime - time >= now)
                {
                    action(ElapsedTime);
                    now = ElapsedTime;
                }
            };
        }
    }
}
