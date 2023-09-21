namespace MCUNetwork.Models
{
    public class MicrocontrollerMemory
    {
        public delegate void ServiceDemandedHandler();
        public delegate void MessageHandler(Message message);

        public event MessageHandler? OnMessageReceived;
        public event MessageHandler? OnMessageIgnored;
        public event ServiceDemandedHandler? OnServiceDemanded;

        private readonly double _size;
        private readonly double _serviceThreshold;
        private readonly List<Message> _values = new(20);
        private double _busy = 0;

        private double FreeSpace { get => _size - _busy; }
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
                _values.Add(message);
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

        public List<Message> Release()
        {
            var list = _values.ToList();

            _values.Clear();
            _busy = 0;

            return list;
        }
    }
}
