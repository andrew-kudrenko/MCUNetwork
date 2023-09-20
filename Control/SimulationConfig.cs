namespace MCUNetwork.Control
{
    internal class SimulationConfig
    {
        public required int SatellitesCount { get; set; }
        public required int Period { get; set; }
        public required int MessageLength { get; set; }
        public required int ServiceDelay { get; set; }
        public required double ServiceThresholdRatio { get; set; }
        public required int MemorySize { get; set; }
        public required int TransferSpeed { get; set; }
    }
}
