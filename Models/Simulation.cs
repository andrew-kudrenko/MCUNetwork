namespace MCUNetwork.Models
{
    public class Simulation
    {
        public readonly MessageCounter MessageCounter = new();
        public readonly List<Satellite> Satellites;
        public readonly Clock Clock;
        public readonly ControlCenter ControlCenter;
        public bool IsRunning { get => Clock.IsRunning; }

        public Simulation(CreateSimulationOptions options)
        {
            Clock = options.Clock;
            ControlCenter = options.ControlCenter;
            Satellites = options.Satellites;
            
            ListenMessageChanges();
        } 

        public async Task Run(int duration)
        {
            await Clock.Run(duration);
        }

        public void Stop()
        {
            Clock.Stop();
        }

        private void ListenMessageChanges()
        {
            foreach (var satellite in Satellites)
            {
                satellite.Microcontroller.OnMessageReceived += _ => MessageCounter.Receive();
                satellite.Microcontroller.OnMessageIgnored += _ => MessageCounter.Ignore();
            }
        }
    }
}
