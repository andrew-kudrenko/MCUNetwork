namespace MCUNetwork.Models
{
    public struct StaticSimulationOptions
    {
        public required int Duration { get; set; }
        public required int Delta { get; set; }
        public required int SatellitesCount { get; set; }
        public required int MessageSize { get; set; }
        public required double ThresholdRatio { get; set; }
        public required int MemorySize { get; set; }
        public required int TransferSpeed { get; set; }
        public required int ServiceEach { get; set; }
    }
}
