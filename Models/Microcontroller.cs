namespace MCUNetwork.Models
{
    public class Microcontroller
    {
        public event Action<Message>? OnMessageReceived;
        public event Action<Message>? OnMessageIgnored;
        public event Action? OnServiceDemanded;
        public event Action? OnServiceIsDone;

        public readonly double Size;
        public readonly MessageCounter MessageCounter = new();
        public double Busy { get => _messages.Sum(m => m.Size); }
        public bool IsServiceDemanded { get; private set; }
        public double FreeSpace { get => Size - Busy; }
        public double BusyAsPercents { get => Busy / (Size / 100); }
        public int CurrentMessagesCount { get => _messages.Count; }

        private readonly double _serviceThreshold;
        private readonly Queue<Message> _messages = new();

        public Microcontroller(double memoryVolume, double thresholdRatio)
        {
            Size = memoryVolume;
            _serviceThreshold = Size * thresholdRatio;
        }

        public bool TryReceive(Message message)
        {
            if (FreeSpace >= message.Size)
            {
                _messages.Enqueue(message);

                if (Busy >= _serviceThreshold)
                {
                    IsServiceDemanded = true;
                    OnServiceDemanded?.Invoke();
                }

                MessageCounter.Receive();
                OnMessageReceived?.Invoke(message);

                return true;
            }

            MessageCounter.Ignore();
            OnMessageIgnored?.Invoke(message);

            return false;
        }

        public IEnumerable<Message> Release()
        {
            while (_messages.Count > 0)
            {
                yield return _messages.Dequeue();
            }

            IsServiceDemanded = false;
            OnServiceIsDone?.Invoke();
        }
    }
}
