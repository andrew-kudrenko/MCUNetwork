namespace MCUNetwork.Models
{
    public class StaticSimulationOptions : SimulationOptions
    {
        public required int SatellitesCount { get; set; }
        public required int MessageSize { get; set; }
        public required double ThresholdRatio { get; set; }
        public required int MemorySize { get; set; }
        public required int TransferSpeed { get; set; }
        public required int ServiceOn { get; set; }
        public required int ReceiveMessageOn { get; set; }
    }
}
