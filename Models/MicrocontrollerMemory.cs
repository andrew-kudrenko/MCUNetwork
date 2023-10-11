namespace MCUNetwork.Models
{
    public class MicrocontrollerMemory
    {
        public event Action<Message>? OnMessageReceived;
        public event Action<Message>? OnMessageIgnored;
        public event Action? OnServiceDemanded;
        public event Action? OnServiceIsDone;

        private readonly double _serviceThreshold;
        private readonly Queue<Message> _messages = new();

        public double Busy { get; private set; } = 0;
        public readonly double Size;
        public double FreeSpace { get => Size - Busy; }
        public double BusyAsPercents { get => Busy / (Size / 100); }
        public int MessagesCount { get => _messages.Count; }
        private bool IsServiceTresholdReached { get => Busy >= _serviceThreshold; }

        public MicrocontrollerMemory(double memoryVolume, double thresholdRatio)
        {
            Size = memoryVolume;
            _serviceThreshold = Size * thresholdRatio;
        }

        public bool TryReceive(Message message)
        {
            if (FreeSpace >= message.Size)
            {
                _messages.Enqueue(message);
                Busy += message.Size;

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

            Busy = 0;
            OnServiceIsDone?.Invoke();
        }
    }
}
