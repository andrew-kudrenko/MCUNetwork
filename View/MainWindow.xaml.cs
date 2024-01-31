using System.IO;
using System.Windows;
using MCUNetwork.Models;
using MCUNetwork.Trace;

namespace MCUNetwork
{
    public partial class MainWindow : Window
    {
        public readonly StaticSimulationOptions StaticSimulationOptions;
        public Simulation? Simulation;

        private readonly SimulationFactory _factory;
        private readonly SimulationCSVSerializer _csvSerializer = new();

        public MainWindow()
        {
            InitializeComponent();

            StaticSimulationOptions = new()
            {
                Duration = 5_000,
                Delta = 10,
                MemorySize = 1_000,
                MessageSize = 200,
                ThresholdRatio = .6,
                SatellitesCount = 5,
                ServiceOn = 10,
                TransferSpeed = 50,
                ReceiveMessageOn = 20,
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
                Simulation = null;

                SetRunButtonState(false);
            } else
            {
                SetRunButtonState(true);
                InitSimulation();

                _csvSerializer.Simulation = Simulation!;

                if (!Directory.Exists("Reports"))
                {
                    Directory.CreateDirectory("Reports");
                }

                using var logStream = new FileStream("Reports/Report.csv", FileMode.OpenOrCreate, FileAccess.Write);
                using var logWriter = new StreamWriter(logStream);

                await logWriter.WriteAsync(_csvSerializer.GetTitle());
                Simulation!.Clock.OnTick(async _ => await logWriter.WriteLineAsync(_csvSerializer.Serialize()));

                await Simulation!.Run(StaticSimulationOptions.Duration);
            }
        }

        private void InitSimulation()
        {
            Simulation = _factory.Create(SimulationKind.Static);
            Simulation.Clock.Delay = GetClockDelay();
            Simulation.Clock.OnTick(_ => ElapsedTime.Text = $"{Simulation.Clock.ElapsedTime} / {StaticSimulationOptions.Duration}");
            Simulation.Clock.OnEnd += () => SetRunButtonState(false);

            SimulationContainer.Simulation = Simulation;
            SpeedSlider.ValueChanged += (sender, args) => Simulation.Clock.Delay = GetClockDelay();
        }

        private void SetRunButtonState(bool isRunning)
        {
            RunButton.Content = isRunning ? "Стоп" : "Пуск";
        }
    }
}