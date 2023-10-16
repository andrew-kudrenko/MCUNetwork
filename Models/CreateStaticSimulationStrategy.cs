namespace MCUNetwork.Models
{
    public class CreateStaticSimulationStrategy : CreateSimulationStrategy
    {
        private readonly StaticSimulationOptions _options;

        public CreateStaticSimulationStrategy(StaticSimulationOptions options)
        {
            _options = options;
        }

        protected override Clock CreateClock() => new() { Delay = 1_000, Delta = _options.Delta };

        protected override ControlCenter CreateControlCenter() => new();

        protected override ExternalDataSource CreateExternalDataSource() => 
            new(new StaticMessageGenerator() { Size = _options.MessageSize });

        protected override int GetMicrocontrollerMemorySize() => _options.MemorySize;

        protected override int GetReceiveMessageOn() => _options.ReceiveMessageOn;

        protected override int GetSatellitesCount() => _options.SatellitesCount;

        protected override int GetServiceOn() => _options.ServiceOn;

        protected override int GetTransferSpeed() => _options.TransferSpeed;

        protected override double GetTresholdRatio() => _options.ThresholdRatio;
    }
}
