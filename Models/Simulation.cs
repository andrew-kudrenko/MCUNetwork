namespace MCUNetwork.Models
{
    public class Simulation
    {
        public readonly List<Satellite> Satellites;
        public readonly Clock Clock;
        public readonly ControlCenter ControlCenter;
        public bool IsRunning { get => Clock.IsRunning; }

        public Simulation(CreateSimulationOptions options)
        {
            Clock = options.Clock;
            ControlCenter = options.ControlCenter;
            Satellites = options.Satellites;
        } 

        public async Task Run(int duration)
        {
            await Clock.Run(duration);
        }

        public void Stop()
        {
            Clock.Stop();
        }
    }
}
