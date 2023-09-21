namespace MCUNetwork.Control
{
    public class Clock
    {
        public delegate void NextTickHandler(long elapsedTime);
        public event NextTickHandler OnNextTick = null!;
        public event NextTickHandler OnEnd = null!;

        public int Delay { get; set; } = 500;

        private bool _isRunning = false;

        public void Run(long duration, int delta)
        {
            if (_isRunning) 
            {
                throw new InvalidOperationException("You have already started a clock");
            }

            _isRunning = true;
            long elapsedTime = 0;

            while (_isRunning)
            {
                if (elapsedTime < duration)
                {
                    OnNextTick.Invoke(elapsedTime);
                    Thread.Sleep(Delay);
                } else
                {
                    _isRunning = false;
                }

                elapsedTime += delta;
            }

            OnEnd.Invoke(elapsedTime);
        }

        public void Stop() => _isRunning = false;
    }
}
