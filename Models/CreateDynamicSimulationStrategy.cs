namespace MCUNetwork.Models
{
    public class CreateDynamicSimulationStrategy : CreateSimulationStrategy
    {
        private readonly Random _random = new();
        private readonly DynamicSimulationOptions _options;

        public CreateDynamicSimulationStrategy(DynamicSimulationOptions options)
        {
            _options = options;
        }

        protected override Clock CreateClock() => new() { Delay = 1_000, Delta = _options.Delta };

        protected override ControlCenter CreateControlCenter() => new();

        protected override ExternalDataSource CreateExternalDataSource() =>
            new(new DynamicMessageGenerator(_options.MessageSize));

        protected override int GetMicrocontrollerMemorySize() => GetRandomFromRange(_options.MemorySize);

        protected override int GetReceiveMessageOn() => GetRandomFromRange(_options.ReceiveMessageOn);

        protected override int GetSatellitesCount() => GetRandomFromRange(_options.SatellitesCount);

        protected override int GetServiceOn() => _options.ServiceOn;

        protected override int GetTransferSpeed() => _options.TransferSpeed;

        protected override double GetTresholdRatio() => GetRandomFromRange(_options.ThresholdRatio);

        private int GetRandomFromRange(Tuple<int, int> range) => _random.Next(range.Item1, range.Item2);
        private double GetRandomFromRange(Tuple<double, double> range) => _random.NextDouble() * (range.Item2 - range.Item1) + range.Item1;
    }
}
