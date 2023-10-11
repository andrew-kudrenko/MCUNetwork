namespace MCUNetwork.Models
{
    public class Pipe
    {
        public event Action<Message>? OnSent;
        public event Action<Message>? OnReceived;

        private Message? _result;
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
            OnSent?.Invoke(message);

            _sent = 0;
            _result = null;

            while (_sent < message.Size)
            {
                _sent += _speed;
                await _clock.Wait();
            }

            _result = message;
        }

        public async Task<Message> Receive()
        {
            while (_result is null)
            {
                await _clock.Wait();
            }

            OnReceived?.Invoke(_result);

            return _result;
        }
    }
}
