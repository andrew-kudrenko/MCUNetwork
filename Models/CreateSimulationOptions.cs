namespace MCUNetwork.Models
{
    public struct CreateSimulationOptions
    {
        public required Clock Clock { get; set; }
        public required ControlCenter ControlCenter { get; set; }
        public required ExternalDataSource ExternalDataSource { get; set; }
        public required List<Satellite> Satellites { get; set; }
    }
}
