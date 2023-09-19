namespace MCUNetwork.Control
{
    class Clock
    {
        public delegate void NextTickHandler(long elapsedTime);
        public event NextTickHandler OnNextTick = null!;

        public int Delay { get; set; } = 500;

        private bool _isRunning = false;

        public void Run()
        {
            if (_isRunning) 
            {
                throw new InvalidOperationException("You have already started a clock");
            }

            _isRunning = true;
            long elapsedTime = 0;

            while (_isRunning)
            {
                OnNextTick.Invoke(elapsedTime);
                Thread.Sleep(Delay);
            }
        }

        public void Stop() => _isRunning = false;
    }
}
