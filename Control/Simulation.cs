using MCUNetwork.Models;

namespace MCUNetwork.Control
{
    public class Simulation
    {
        public readonly ControlCenter ControlCenter = new();
        
        private SimulationConfig _config;
        private Clock _clock = new();

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
            _clock.Run(86_400, _config.ServiceDelay);
        }

        private void Init()
        {
            for (int i = 0; i < _config.SatellitesCount; i++)
            {
                var satellite = new Microcontroller(_config.MemorySize, _config.ServiceThresholdRatio);
                ControlCenter.AddSatellite(satellite);
            }
        }
    }
}
