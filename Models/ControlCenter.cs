using System.Collections.ObjectModel;

namespace MCUNetwork.Models
{
    public class ControlCenter
    {
        public readonly ObservableCollection<Microcontroller> Satellites = new();

        public void AddSatellite(Microcontroller satellite)
        {
            Satellites.Add(satellite);
            satellite.Memory.OnServiceDemanded += () => Service(satellite);
        }

        private List<Message> Service(Microcontroller satellite)
        {
            return satellite.Memory.Release();
        }
    }
}
