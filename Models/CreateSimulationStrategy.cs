namespace MCUNetwork.Models
{
    public abstract class CreateSimulationStrategy
    {
        protected Clock clock = null!;
        protected ControlCenter controlCenter = null!;
        protected ExternalDataSource externalDataSource = null!;
        protected List<Satellite> satellites = null!;

        public Simulation Create()
        {
            Init();

            var simulation = new Simulation(new()
            {
                Clock = clock,
                ControlCenter = controlCenter,
                ExternalDataSource = externalDataSource,
                Satellites = satellites
            });

            return simulation;
        }

        protected void Init()
        {
            satellites = new();
            controlCenter = CreateControlCenter();
            clock = CreateClock();
            externalDataSource = CreateExternalDataSource();

            InitSatellites();
            ScheduleService();
        }

        protected abstract ControlCenter CreateControlCenter();
        protected abstract Clock CreateClock();
        protected abstract ExternalDataSource CreateExternalDataSource();

        protected abstract int GetSatellitesCount();
        protected abstract int GetServiceOn();
        protected abstract int GetMicrocontrollerMemorySize();
        protected abstract double GetTresholdRatio();
        protected abstract int GetTransferSpeed();
        protected abstract int GetReceiveMessageOn();

        private void ScheduleService() => clock.OnTick(async _ => await controlCenter.Service(), GetServiceOn());

        private void InitSatellites()
        {
            for (int i = 0; i < GetSatellitesCount(); i++)
            {
                var satellite = new Satellite()
                {
                    Microcontroller = new Microcontroller(GetMicrocontrollerMemorySize(), GetTresholdRatio()),
                    Pipe = new Pipe(clock, GetTransferSpeed()),
                };

                clock.OnTick(
                    _ => satellite.Microcontroller.TryReceive(externalDataSource.Send()),
                    GetReceiveMessageOn()
                );
                satellite.Microcontroller.OnServiceDemanded += () => controlCenter.DemandService(satellite);

                satellites.Add(satellite);
            }
        }
    }
}
