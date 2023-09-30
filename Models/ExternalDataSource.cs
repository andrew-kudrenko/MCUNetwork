namespace MCUNetwork.Models
{
    public class ExternalDataSource
    {
        private readonly List<(Microcontroller, int)> _microcontrollers = new();
        private readonly Clock _clock;
        private readonly Random _random = new();

        public ExternalDataSource(Clock clock) 
        {
            _clock = clock;
        }

        public void Start()
        {
            foreach (var (microcontroller, updateOn) in _microcontrollers)
            {
                _clock.ExecuteEach(() => SendMessage(microcontroller), updateOn);
            }
        }

        public void Register(Microcontroller microcontroller)
        {
            _microcontrollers.Add((microcontroller, GetUpdateOn()));
        }

        private int GetUpdateOn() => _random.Next(10, 180);

        private Message GenerateRandomMessage() => new(_random.Next(25, 250));

        private void SendMessage(Microcontroller microcontroller)
        {
            microcontroller.Memory.TryReceive(GenerateRandomMessage());
        }
    }
}
