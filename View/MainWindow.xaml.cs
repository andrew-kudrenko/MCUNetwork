using System.Windows;
using MCUNetwork.Models;

namespace MCUNetwork
{
    public partial class MainWindow : Window
    {
        public readonly StaticSimulationOptions StaticSimulationOptions;
        public Simulation? Simulation;

        private readonly SimulationFactory _factory;

        public MainWindow()
        {
            InitializeComponent();

            StaticSimulationOptions = new()
            {
                Duration = 86_400,
                Delta = 10,
                MemorySize = 1000,
                MessageSize = 200,
                ThresholdRatio = .6,
                SatellitesCount = 7,
                ServiceOn = 10,
                TransferSpeed = 50,
                ReceiveMessageOn = 60,
            };

            _factory = new(StaticSimulationOptions);
            SettingsPanel.Value = StaticSimulationOptions;

            RunButton.Click += async (sender, args) => await OnClickRun();
        }

        private int GetClockDelay() => Math.Max(100 - ((int)SpeedSlider.Value), 1);

        private async Task OnClickRun()
        {
            if (Simulation is not null && Simulation.IsRunning)
            {
                Simulation.Stop();
                Simulation = null!;

                RunButton.Content = "Пуск";
            } else
            {
                RunButton.Content = "Остановка";

                Simulation = _factory.Create(SimulationKind.Static);
                Simulation.Clock.Delay = GetClockDelay();
                Simulation.Clock.OnTick(_ => ElapsedTime.Text = $"{Simulation.Clock.ElapsedTime} / {StaticSimulationOptions.Duration}");

                SimulationContainer.Simulation = Simulation;
                SpeedSlider.ValueChanged += (sender, args) => Simulation.Clock.Delay = GetClockDelay();

                await Simulation.Run(StaticSimulationOptions.Duration);
            }
        }
    }
}