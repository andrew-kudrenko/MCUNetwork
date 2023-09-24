using System.Windows;
using MCUNetwork.Control;

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
            RunButton.Click += (_, __) =>
            {
                if (Simulation.isRunning)
                {
                    Simulation.Stop();
                    Console.WriteLine("Simulation has benn stopped");
                } else
                {
                    Simulation.Run();
                    Console.WriteLine("Simulation is running!");
                }
            };
        }
    }
}