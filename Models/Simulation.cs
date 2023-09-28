namespace MCUNetwork.Models
{
    public class Simulation
    {
        public bool IsRunning { get; private set; } = false;
        public readonly Clock Clock = new();
        public readonly ControlCenter ControlCenter;
        
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
            ControlCenter = new(Clock);

            Init();
        } 

        public async Task Run()
        {
            IsRunning = true;

            long duration = 86_400L;

            Clock.Delay = 80;
            Clock.ExecuteUntil(100, duration, SendMockMessage);
            
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
            var mc = ControlCenter.Satellites.ElementAt(at);
            
            mc.Memory.TryReceive(new(_random.Next(100) + 50));
        }
    }
}
