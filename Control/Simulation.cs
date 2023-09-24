using MCUNetwork.Models;

namespace MCUNetwork.Control
{
    public class Simulation
    {
        public bool isRunning { get; private set; } = false;
        public readonly ControlCenter ControlCenter = new();
        
        private SimulationConfig _config;
        private Clock _clock = new();
        private readonly Random _random = new Random();

        public Simulation()
        {
            _config = new() { 
                MemorySize = 1_000, 
                MessageLength = 200, 
                Period = 20, 
                SatellitesCount = 5, 
                ServiceDelay = 10, 
                ServiceThresholdRatio = 0.6, 
                TransferSpeed = 50,
            };

            Init();
        } 

        public void Run()
        {
            isRunning = true;

            Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                // here will should be the clock's launching
            });

            _clock.ExecuteUntil(5, 50, SendMockMessage);

            _clock.Run(86_400, _config.ServiceDelay);
        }

        public void Stop()
        {
            isRunning = false;
            _clock.Stop();
        }

        private void Init()
        {
            for (int i = 0; i < _config.SatellitesCount; i++)
            {
                var satellite = new Microcontroller(_config.MemorySize, _config.ServiceThresholdRatio);
                ControlCenter.AddSatellite(satellite);
            }
        }

        private void SendMockMessage()
        {
            Console.WriteLine("Sending...");
            var at = _random.Next(ControlCenter.Satellites.Count);
            try
            {
                var isReceived = ControlCenter.Satellites[at].Memory.TryReceive(new(_random.Next(20) + 40));
                Console.WriteLine($"Received mock message {isReceived}");
            } catch (Exception ex)
            {
                Console.WriteLine($"Failed to send {ex.Message}");
            }
        }
    }
}
