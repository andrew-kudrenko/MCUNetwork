using MCUNetwork.Control;

namespace MCUNetwork.Models
{
    public class ControlCenter
    {
        public delegate void ReceiveMessageHandler(Message message);
        public event ReceiveMessageHandler? OnReceiveMessage;
        public int ReceivedMessages { get; private set; } = 0;

        public readonly List<Microcontroller> Satellites = new();
        private readonly Clock _clock;

        public ControlCenter(Clock clock) => _clock = clock;

        public void AddSatellite(Microcontroller satellite)
        {
            var pipe = new Pipe(_clock, 150);

            pipe.OnMessageSent += message =>
            {
                ReceivedMessages++;
                OnReceiveMessage?.Invoke(message);
            };

            Satellites.Add(satellite);
            satellite.Memory.OnServiceDemanded += async () =>
            {
                await Task.Delay(1_000);
                Service(satellite, pipe);
            };
        }

        private static void Service(Microcontroller satellite, Pipe pipe)
        {
            int serviced = 0;

            foreach (var message in satellite.Memory.Release())
            {
                pipe.Send(message);
                serviced++;
            }
        }
    }
}
