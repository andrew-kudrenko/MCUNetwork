namespace MCUNetwork.Models
{
    public class SimulationFactory
    {
        private readonly Dictionary<SimulationKind, CreateSimulationStrategy> _createStrategies;
        private readonly StaticSimulationOptions _staticOptions;
        private readonly DynamicSimulationOptions _dynamicOptions;

        public SimulationFactory(StaticSimulationOptions options)
        {
            _staticOptions = options;
            _createStrategies = new() { 
                { SimulationKind.Static, new CreateStaticSimulationStrategy(_staticOptions) },
                { SimulationKind.Dynamic, new CreateDynamicSimulationStrategy(_dynamicOptions) }
            };
        }

        public Simulation Create(SimulationKind kind) => _createStrategies[kind].Create();
    }
}
