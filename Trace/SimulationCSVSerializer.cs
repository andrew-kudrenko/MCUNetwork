using MCUNetwork.Models;

namespace MCUNetwork.Trace
{
    public class SimulationCSVSerializer
    {
        public Simulation Simulation { get; set; }

        private const string _delimiter = ",";

        public SimulationCSVSerializer(Simulation simulation) => Simulation = simulation;

        public string Serialize()
        {
            return string.Join(
                _delimiter, 
                Simulation.Clock.ElapsedTime, 
                GetBusy()
            );
        }

        private string GetBusy() => 
            string.Join(", ", Simulation.Satellites.Select(s => s.Microcontroller.BusyAsPercents));
    }
}
