namespace MCUNetwork.Models
{
    public class ControlCenter
    {
        public delegate void ReceiveMessageHandler(Message message);
        public event ReceiveMessageHandler? OnReceiveMessage;
        public event ReceiveMessageHandler? OnIgnoreMessage;
        public int ReceivedMessages { get; private set; } = 0;
        public int IgnoredMessages { get; private set; } = 0;

        public Queue<(Microcontroller, Pipe)> ServiceRequestors = new();
        public int ServiceDelay { get; set; } = 750;

        public readonly List<Microcontroller> Satellites = new();
        private readonly Clock _clock;

        public ControlCenter(Clock clock)
        {
            _clock = clock;
            Init();
        }

        public void Register(Microcontroller satellite)
        {
            var pipe = new Pipe(_clock, 100);

            pipe.OnMessageSent += message =>
            {
                ReceivedMessages++;
                OnReceiveMessage?.Invoke(message);
            };

            Satellites.Add(satellite);
            satellite.Memory.OnMessageIgnored += message => 
            {
                IgnoredMessages++;
                OnIgnoreMessage?.Invoke(message);
            };
            satellite.Memory.OnServiceDemanded += () => ServiceRequestors.Enqueue((satellite, pipe));
        }

        private void Init()
        {
            _clock.ExecuteEach(Service, ServiceDelay);
        }

        private void Service()
        {
            while (ServiceRequestors.Count > 0)
            {
                var (requestor, pipe) = ServiceRequestors.Dequeue();
                SendMessages(requestor, pipe);
            }
        }

        private static void SendMessages(Microcontroller satellite, Pipe pipe)
        {
            foreach (var message in satellite.Memory.Release())
            {
                pipe.Send(message);
            }
        }
    }
}
