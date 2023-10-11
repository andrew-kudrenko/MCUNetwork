using System.Windows;
using MCUNetwork.Models;

namespace MCUNetwork
{
    public partial class MainWindow : Window
    {
        public Simulation Simulation;

        private readonly SimulationFactory _factory = new();

        public MainWindow()
        {
            InitializeComponent();

            Simulation = _factory.Create();
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
            Simulation.Clock.ScheduleEach(elapsedTicks => ElapsedTime.Text = elapsedTicks.ToString(), 1);
            await Simulation.Run();
        }
    }
}