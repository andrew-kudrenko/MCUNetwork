using System.Windows;
using MCUNetwork.Models;

namespace MCUNetwork
{
    public partial class MainWindow : Window
    {
        public readonly Simulation Simulation = new();

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            RunButton.Click += async (sender, args) =>
            {
                if (Simulation.IsRunning)
                {
                    Simulation.Stop();
                } else
                {
                    await Simulation.Run();
                }
            };

            Simulation.Clock.OnNextTick += time => ElapsedTime.Text = time.ToString();
            SpeedSlider.ValueChanged += (sender, args) =>
            {
                Simulation.Clock.Delay = 101 - ((int) args.NewValue);
            };

            Simulation.Clock.Delay = 101 - ((int) SpeedSlider.Value);

            SimulationContainer.Simulation = Simulation;
        }
    }
}