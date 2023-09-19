namespace MCUNetwork.Models
{
    class ControlCenter
    {
        private readonly int _serviceDelayMilliseconds = 1_000;
        private readonly Queue<Microcontroller> _serviceDemandedSatellites = new();
        private readonly List<Microcontroller> _satellites = new(10);

        public void AddSatellite(Microcontroller satellite)
        {
            _satellites.Add(satellite);
            satellite.Memory.OnServiceDemanded += () => Service(satellite);
        }

        private void ScheduleService(Microcontroller satellite)
        {
            _serviceDemandedSatellites.Enqueue(satellite);
        }

        private List<Message> Service(Microcontroller satellite)
        {
            return satellite.Memory.Release();
        }
    }
}
