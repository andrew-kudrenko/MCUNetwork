namespace MCUNetwork.Models
{
    public class SimulationFactory
    {
        private readonly Dictionary<SimulationKind, CreateSimulationStrategy> _createStrategies;
        private readonly StaticSimulationOptions _options;

        public SimulationFactory(StaticSimulationOptions options)
        {
            _options = options;
            _createStrategies = new() { 
                { SimulationKind.Static, new CreateStaticSimulationStrategy(_options) } 
            };
        }

        public Simulation Create(SimulationKind kind) => _createStrategies[kind].Create();
    }
}
