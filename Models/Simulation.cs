namespace MCUNetwork.Models
{
    public class Simulation
    {
        public readonly List<Satellite> Satellites = new();
        public readonly Clock Clock;
        public readonly ControlCenter ControlCenter;
        public bool IsRunning { get => Clock.IsRunning; }

        private readonly ExternalDataSource _externalDataSource;
        private readonly SimulationConfig _config;

        public Simulation(CreateSimulationOptions options, SimulationConfig config)
        {
            _config = config;
            Clock = options.Clock;
            ControlCenter = options.ControlCenter;
            _externalDataSource = options.ExternalDataSource;

            Init();
        } 

        public async Task Run()
        {
            await Clock.Run(_config.Duration);
        }

        public void Stop()
        {
            Clock.Stop();
        }

        private void Init()
        {
            InitSatellites();
            Clock.ScheduleEach(async elapsedTicks => await ControlCenter.Service(), _config.ServiceEach);
        }

        private void InitSatellites()
        {
            for (int i = 0; i < _config.SatellitesCount; i++)
            {
                var satellite = new Satellite() 
                {
                    Microcontroller = new Microcontroller(_config.MemorySize, _config.ThresholdRatio),
                    Pipe = new Pipe(Clock, _config.TransferSpeed),
                    ReceiveMessageEach = _config.ReceiveMessageEach,
                };

                Clock.ScheduleEach(elapsedTicks => satellite.Microcontroller.Memory.TryReceive(_externalDataSource.Send()), satellite.ReceiveMessageEach);
                satellite.Microcontroller.Memory.OnServiceDemanded += () => ControlCenter.DemandService(satellite);
                
                Satellites.Add(satellite);
            }
        }
    }
}
