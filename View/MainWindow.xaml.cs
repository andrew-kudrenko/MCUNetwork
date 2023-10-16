using System.Windows;
using MCUNetwork.Models;

namespace MCUNetwork
{
    public partial class MainWindow : Window
    {
        public Simulation Simulation;

        private readonly StaticSimulationOptions _simulationOptions;
        private readonly SimulationFactory _factory;

        public MainWindow()
        {
            InitializeComponent();

            _simulationOptions = new()
            {
                Duration = 86_400,
                Delta = 10,
                MemorySize = 1000,
                MessageSize = 200,
                ThresholdRatio = .6,
                SatellitesCount = 7,
                ServiceOn = 10,
                TransferSpeed = 50,
                ReceiveMessageOn = 45,
            };
            _factory = new(_simulationOptions);

            Simulation = _factory.Create(SimulationKind.Static);
            Init();
        }

        private void Init()
        {
            RunButton.Click += async (sender, args) => await OnClickRun();

            SpeedSlider.ValueChanged += (sender, args) => Simulation.Clock.Delay = GetClockDelay();
            Simulation.Clock.Delay = GetClockDelay();

            SimulationContainer.Simulation = Simulation;
        }

        private int GetClockDelay() => Math.Max(100 - ((int)SpeedSlider.Value), 1);

        private async Task OnClickRun()
        {
            Simulation.Clock.ScheduleEach(elapsedTicks => ElapsedTime.Text = (elapsedTicks * Simulation.Clock.Delta).ToString());
            await Simulation.Run(_simulationOptions.Duration);
        }
    }
}