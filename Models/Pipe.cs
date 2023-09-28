namespace MCUNetwork.Models
{
    public class Pipe
    {
        public delegate void SentMessageHandler(Message message);
        public event SentMessageHandler? OnMessageSent;

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
                        OnMessageSent?.Invoke(_messages.Dequeue());
                    }
                } else
                {
                    _sentVolume = 0;
                }
            };
        }
    }
}
