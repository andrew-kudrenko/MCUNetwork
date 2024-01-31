namespace MCUNetwork.Models
{
    public class Pipe
    {
        public event Action<Message>? OnSent;
        public event Action<Message>? OnReceived;
        public event Action<int>? OnProgress;

        private readonly Clock _clock;
        private readonly int _speed;

        public Pipe(Clock clock, int capacity)
        {
            _clock = clock;
            _speed = capacity;
        }

        public async Task Send(Message message)
        {
            OnSent?.Invoke(message);

            while (message.Size > 0)
            {
                var sizeBefore = message.Size;
                message.Size = Math.Max(0, message.Size - _speed);

                OnProgress?.Invoke(sizeBefore - message.Size);

                await _clock.WaitTicks();
            }

            OnReceived?.Invoke(message);
        }
    }
}
