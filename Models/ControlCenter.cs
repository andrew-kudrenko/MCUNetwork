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
        public int ServiceDelay { get; set; } = 350;

        public readonly List<(Microcontroller, Pipe)> Satellites = new();
        private readonly Clock _clock;

        public ControlCenter(Clock clock)
        {
            _clock = clock;
            Init();
        }

        public void Register((Microcontroller, Pipe) pair)
        {
            pair.Item2.OnReceived += message =>
            {
                ReceivedMessages++;
                OnReceiveMessage?.Invoke(message);
            };

            Satellites.Add(pair);
            pair.Item1.Memory.OnMessageIgnored += message => 
            {
                IgnoredMessages++;
                OnIgnoreMessage?.Invoke(message);
            };
            pair.Item1.Memory.OnServiceDemanded += () => ServiceRequestors.Enqueue(pair);
        }

        private void Init()
        {
            _clock.ExecuteEach(Service, ServiceDelay);
        }

        private void Service()
        {
            while (ServiceRequestors.Count > 0)
            {
                SendMessages(ServiceRequestors.Dequeue());
            }
        }

        private static void SendMessages((Microcontroller, Pipe) pair)
        {
            foreach (var message in pair.Item1.Memory.Release())
            {
                pair.Item2.Send(message);
            }
        }
    }
}
