using MCUNetwork.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MCUNetwork.View
{
    public partial class SimulationContainer : UserControl
    {
        public readonly static DependencyProperty SimulationProperty = DependencyProperty.Register(
            "Simulation",
            typeof (Simulation),
            typeof (SimulationContainer),
            new PropertyMetadata(null, new(OnChangeSimulation))
        );
        
        public Simulation Simulation
        {
            get => (Simulation) GetValue(SimulationProperty);
            set => SetValue(SimulationProperty, value);
        }

        public SimulationContainer()
        {
            InitializeComponent();
        }

        private static void OnChangeSimulation(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is SimulationContainer view)
            {
                view.Simulation = (Simulation) args.NewValue;
                view.DataContext = view.Simulation;

                if (view.IsLoaded)
                {
                    view.Init(null!, null!);
                } else
                {
                    view.Loaded += view.Init;
                }
            }
        }

        private void Init(object sender, RoutedEventArgs args)
        {
            PositionSatellites();
            PositionControlCenter();
            ControlCenterView.ControlCenter = Simulation.ControlCenter;
        }

        private Point GetCenter() => new (ActualWidth / 2, ActualHeight / 2);

        private void PositionControlCenter()
        {
            var center = GetCenter();
            ControlCenterView.RenderTransformOrigin = center;
            ControlCenterView.RenderTransform = new TranslateTransform() 
            {
                X = center.X - (ControlCenterView.ActualWidth / 2),
                Y = center.Y - (ControlCenterView.ActualHeight / 2)
            };
        }

        private void PositionSatellites()
        {
            if (Simulation.ControlCenter.Satellites.Count == 0)
            {
                return;
            }

            var radius = 180D;
            var shift = GetCenter();
            var angleStep = 360D / Simulation.ControlCenter.Satellites.Count;

            for (int i = 0; i < Simulation.ControlCenter.Satellites.Count; i++)
            {
                var angle = (angleStep * i) * (Math.PI / 180D);

                var x = shift.X + radius * Math.Sin(angle);
                var y = shift.Y + radius * Math.Cos(angle);

                var child = new MicrocontrollerView()
                {
                    Microcontroller = Simulation.ControlCenter.Satellites[i],
                    RenderTransform = new TranslateTransform(x, y),
                    RenderTransformOrigin = new Point(.5, .5)
                };

                ContainerView.Children.Add(child);
            }
        }
    }
}
