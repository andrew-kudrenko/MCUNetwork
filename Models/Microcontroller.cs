namespace MCUNetwork.Models
{
    internal class Microcontroller
    {
        public event EventHandler OnDemandService = null!;

        private readonly double _memorySize;
        private readonly double _serviceMemorySizeThreshold;
        private readonly Stack<Message> _messages = new (25);
        private double _busyMemorySize = 0;

        public Microcontroller(double memoryVolume, double thresholdRatio) 
        {
            _memorySize = memoryVolume;    
            _serviceMemorySizeThreshold = _memorySize * thresholdRatio;
        }

        public IList<Message> ReleaseMessages()
        {
            var messages = _messages.ToList();

            _messages.Clear();
            _busyMemorySize = 0;

            return messages;
        }

        public bool TryReceiveMessage(Message message) 
        {
            if (HasEnoughFreeMemory(message)) 
            {
                _messages.Push(message);
                IncreaseBusyMemorySize(message.Size);

                return true;
            }

            return false;
        }

        private void IncreaseBusyMemorySize(double size)
        {
            _busyMemorySize += size;

            if (IsServiceThresholdReached()) 
            {
                OnDemandService?.Invoke(null, null!);
            }

            if (_busyMemorySize > _memorySize) {
                _busyMemorySize = _memorySize;
            }
        }

        private bool IsServiceThresholdReached() => _busyMemorySize >= _serviceMemorySizeThreshold;

        private bool HasEnoughFreeMemory(Message message) => _busyMemorySize + message.Size <= _memorySize;
    }
}
