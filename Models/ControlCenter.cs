using MCUNetwork.Control;

namespace MCUNetwork.Models
{
    public class ControlCenter
    {
        public delegate void ReceiveMessageHandler(Message message);
        public event ReceiveMessageHandler? OnReceiveMessage;
        public int ReceivedMessages { get; private set; }

        public readonly List<(Microcontroller, Pipe)> Satellites = new();
        private readonly Clock _clock;

        public ControlCenter(Clock clock) => _clock = clock;

        public void AddSatellite(Microcontroller satellite)
        {
            var pipe = new Pipe(_clock, 50);

            pipe.OnMessageSent += message =>
            {
                Console.WriteLine($"Message is sent: {message.Size}");
                ReceivedMessages++;
                OnReceiveMessage?.Invoke(message);
            };

            Satellites.Add((satellite, pipe));
            satellite.Memory.OnServiceDemanded += async () =>
            {
                await Task.Delay(2_000);
                await Service(satellite, pipe);
            };
        }

        private static async Task Service(Microcontroller satellite, Pipe pipe)
        {
            foreach (var message in satellite.Memory.Release())
            {
                await Task.Delay(500);
                pipe.Send(message);
            }
        }
    }
}
