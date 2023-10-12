namespace MCUNetwork.Models
{
    public class Pipe
    {
        public event Action<Message>? OnSent;
        public event Action<Message>? OnReceived;

        private readonly Clock _clock;
        private readonly double _speed;
        private double _sent = 0;

        public Pipe(Clock clock, double capacity)
        {
            _clock = clock;
            _speed = capacity;
        }

        public async Task Send(Message message)
        {
            _sent = 0;
            OnSent?.Invoke(message);

            while (_sent < message.Size)
            {
                _sent += _speed;
                await _clock.Wait();
            }

            OnReceived?.Invoke(message);
        }
    }
}
