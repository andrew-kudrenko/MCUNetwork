namespace MCUNetwork.Models
{
    public class MicrocontrollerMemory
    {
        public delegate void ServiceHandler();
        public delegate void MessageHandler(Message message);

        public event MessageHandler? OnMessageReceived;
        public event MessageHandler? OnMessageIgnored;
        public event ServiceHandler? OnServiceDemanded;
        public event ServiceHandler? OnServiceIsDone;

        private readonly double _size;
        private readonly double _serviceThreshold;
        private readonly Queue<Message> _messages = new();
        private double _busy = 0;

        public double FreeSpace { get => _size - _busy; }
        public double BusyAsPercents { get => _busy / (_size / 100); }
        public int MessagesCount { get => _messages.Count; }
        private bool IsServiceTresholdReached { get => _busy >= _serviceThreshold; }

        public MicrocontrollerMemory(double memoryVolume, double thresholdRatio)
        {
            _size = memoryVolume;
            _serviceThreshold = _size * thresholdRatio;
        }

        public bool TryReceive(Message message)
        {
            if (FreeSpace >= message.Size)
            {
                _messages.Enqueue(message);
                _busy += message.Size;

                if (IsServiceTresholdReached)
                {
                    OnServiceDemanded?.Invoke();
                }

                OnMessageReceived?.Invoke(message);

                return true;
            }

            OnMessageIgnored?.Invoke(message);

            return false;
        }

        public IEnumerable<Message> Release()
        {
            while (_messages.Count > 0)
            {
                yield return _messages.Dequeue();
            }

            _busy = 0;
            OnServiceIsDone?.Invoke();
        }
    }
}
