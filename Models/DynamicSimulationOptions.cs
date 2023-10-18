namespace MCUNetwork.Models
{
    public struct DynamicSimulationOptions
    {
        public required int Duration { get; set; }
        public required int Delta { get; set; }
        public required Tuple<int, int> SatellitesCount { get; set; }
        public required Tuple<int, int> MessageSize { get; set; }
        public required Tuple<double, double> ThresholdRatio { get; set; }
        public required Tuple<int, int> MemorySize { get; set; }
        public required int TransferSpeed { get; set; }
        public required int ServiceOn { get; set; }
        public required Tuple<int, int> ReceiveMessageOn { get; set; }
    }
}
