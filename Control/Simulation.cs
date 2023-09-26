using MCUNetwork.Models;

namespace MCUNetwork.Control
{
    public class Simulation
    {
        public bool IsRunning { get; private set; } = false;
        public readonly ControlCenter ControlCenter = new();
        
        private readonly SimulationConfig _config;
        private readonly Clock _clock = new();
        private readonly Random _random = new();

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

        public async Task Run()
        {
            IsRunning = true;

            _clock.ExecuteUntil(10, 10_000, SendMockMessage);
            
            await _clock.Run(86_400, _config.ServiceDelay);
        }

        public void Stop()
        {
            IsRunning = false;
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
            var at = _random.Next(ControlCenter.Satellites.Count)            
            ControlCenter.Satellites[at].Memory.TryReceive(new(_random.Next(20) + 40));
        }
    }
}
