using System.Windows;
using MCUNetwork.Models;

namespace MCUNetwork
{
    public partial class MainWindow : Window
    {
        public Simulation Simulation { get; } = new Simulation();

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            SatellitesBox.ItemsSource = Simulation.ControlCenter.Satellites;
            CenterView.ControlCenter = Simulation.ControlCenter;

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
        }
    }
}