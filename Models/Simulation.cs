namespace MCUNetwork.Models
{
    public class Simulation
    {
        public bool IsRunning { get => Clock.IsRunning; }
        public readonly Clock Clock = new();
        public readonly ControlCenter ControlCenter;
        
        private readonly SimulationConfig _config;
        private readonly ExternalDataSource _externalDataSource;

        public Simulation()
        {
            _config = new() { 
                MemorySize = 1_000, 
                MessageLength = 75, 
                Period = 20, 
                SatellitesCount = 9, 
                ServiceDelay = 10, 
                ServiceThresholdRatio = 0.45, 
                TransferSpeed = 50,
            };
            ControlCenter = new(Clock);
            _externalDataSource = new(Clock);

            Init();
        } 

        public async Task Run()
        {
            var running = Clock.Run(86_400L, 10);
            _externalDataSource.Start();
            await running;
        }

        public void Stop()
        {
            Clock.Stop();
        }

        private void Init()
        {
            for (int i = 0; i < _config.SatellitesCount; i++)
            {
                var satellite = new Microcontroller(_config.MemorySize, _config.ServiceThresholdRatio);
                
                ControlCenter.Register(satellite);
                _externalDataSource.Register(satellite);
            }
        }
    }
}
