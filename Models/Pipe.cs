namespace MCUNetwork.Models
{
    public class Pipe
    {
        public delegate void MessageHandler(Message message);
        
        public event MessageHandler? OnSent;
        public event MessageHandler? OnReceived;

        private readonly double _capacity;
        private readonly Clock _clock;
        private readonly Queue<Message> _messages = new();
        private double _sentVolume = 0;

        public Pipe(Clock clock, double capacity)
        {
            _clock = clock;
            _capacity = capacity;

            Init();
        }

        public void Send(Message message)
        {
            _messages.Enqueue(message);
            OnSent?.Invoke(message);
        }

        private void Init()
        {
            _clock.OnNextTick += elapsedTime =>
            {
                if (_messages.Count > 0 && _messages.TryPeek(out Message? message))
                {
                    _sentVolume += _capacity;

                    if (_sentVolume >= message.Size)
                    {
                        _sentVolume -= message.Size;
                        OnReceived?.Invoke(_messages.Dequeue());
                    }
                } else
                {
                    _sentVolume = 0;
                }
            };
        }
    }
}
