namespace MCUNetwork.Models
{
    public class SimulationFactory
    {
        private readonly StaticSimulationOptions _options;

        public SimulationFactory()
        {
            _options = new()
            {
                Duration = 86_400,
                Delta = 10,
                MemorySize = 1000,
                MessageSize = 200,
                ThresholdRatio = .6,
                SatellitesCount = 7,
                ServiceEach = 10,
                TransferSpeed = 50,
            };
        }

        public Simulation Create()
        {
            var simulation = new Simulation(
                new()
                {
                    Clock = new() { Delay = 1, Delta = _options.Delta },
                    ControlCenter = new(),
                    ExternalDataSource = new(new StaticMessageGenerator() { Size = _options.MessageSize }),
                }, 
                new()
                { 
                    Duration = 86_400,
                    Delta = 10,
                    MemorySize = 800,
                    MessageSize = 200,
                    SatellitesCount = 7,
                    ServiceEach = 10,
                    ThresholdRatio = .6,
                    TransferSpeed = 50,
                    ReceiveMessageEach = 100,
                });

            return simulation;
        }
    }
}
