using MCUNetwork.Models;

namespace MCUNetwork.Control
{
    public class Simulation
    {
        public bool IsRunning { get; private set; } = false;
        public readonly ControlCenter ControlCenter = new();
        public readonly Clock Clock = new();
        
        private readonly SimulationConfig _config;
        private readonly Random _random = new();

        public Simulation()
        {
            _config = new() { 
                MemorySize = 1_000, 
                MessageLength = 75, 
                Period = 20, 
                SatellitesCount = 5, 
                ServiceDelay = 10, 
                ServiceThresholdRatio = 0.45, 
                TransferSpeed = 50,
            };

            Init();
        } 

        public async Task Run()
        {
            IsRunning = true;

            long duration = 86_400L;

            Clock.Delay = 1;
            Clock.ExecuteUntil(50, duration, SendMockMessage);
            
            await Clock.Run(duration, _config.ServiceDelay);
        }

        public void Stop()
        {
            IsRunning = false;
            Clock.Stop();
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
            var at = _random.Next(ControlCenter.Satellites.Count);          
            ControlCenter.Satellites[at].Memory.TryReceive(new(_random.Next(100) + 50));
        }
    }
}
