using MCUNetwork.Models;

namespace MCUNetwork.Trace
{
    public class SimulationCSVSerializer
    {
        public Simulation Simulation { get; set; } = null!;

        private const string _delimiter = "; ";

        public string GetTitle()
        {
            var satellitesRange = Enumerable.Range(0, Simulation.Satellites.Count);

            return Join(
                "T",
                "СВ",
                "СП",
                "СУ",
                "СУ%",
                Join(satellitesRange.Select(i => $"З{i}(%)")),
                Join(satellitesRange.Select(i => $"У{i}(%)")),
                Join(satellitesRange.Select(i => $"ЗС{i}(%)"))
            );
        }

        public string Serialize()
        {
            return Join(
                Simulation.Clock.ElapsedTime.ToString(),
                Simulation.MessageCounter.Total.ToString(),
                Simulation.MessageCounter.Received.ToString(),
                Simulation.MessageCounter.Ignored.ToString(),
                $"{Simulation.MessageCounter.IgnoredAsPercents}%",
                GetBusy(),
                GetIgnored(),
                GetServiceDemanded()
             );
        }

        private string GetBusy() => Join(Simulation.Satellites.Select(s => s.Microcontroller.BusyAsPercents));

        private string GetIgnored() => Join(Simulation.Satellites.Select(s => $"{s.Microcontroller.MessageCounter.IgnoredAsPercents}%"));

        private string GetServiceDemanded() => Join(Simulation.Satellites.Select(s => s.Microcontroller.IsServiceDemanded));

        private static string Join<T>(IEnumerable<T> values) => string.Join(_delimiter, values);

        private static string Join(params string?[] values) => string.Join(_delimiter, values);
    }
}
