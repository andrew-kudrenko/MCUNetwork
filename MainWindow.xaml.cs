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
        }
    }
}