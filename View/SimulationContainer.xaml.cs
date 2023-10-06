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
                view.Loaded += view.Init;
            }
        }

        private void Init(object sender, RoutedEventArgs args)
        {
            ControlCenterView.ControlCenter = Simulation.ControlCenter;
            PositionControlCenter();
            PositionSatellites();
        }

        private Point GetCenter() => new (ActualWidth / 2, ActualHeight / 2);

        private void PositionControlCenter()
        {
            var center = GetCenter();

            ControlCenterView.Width = GetControlCenterWidth();
            ControlCenterView.RenderTransform = new TranslateTransform() 
            {
                X = center.X - (ControlCenterView.Width / 2),
                Y = center.Y - (ControlCenterView.ActualHeight / 2)
            };
        }

        private double GetControlCenterWidth() => ActualWidth / 4;

        private double GetSatelliteWidth() => ActualWidth / 10;

        private double GetPipeWidth() => ActualWidth / 12;

        private void PositionSatellites()
        {
            if (Simulation.ControlCenter.Satellites.Count == 0)
            {
                return;
            }

            double angleStep = 360d / Simulation.ControlCenter.Satellites.Count;
            var satelliteWidth = GetSatelliteWidth();
            double satelliteHeight = satelliteWidth * 1.5;
            var pipeWidth = GetPipeWidth();
            var center = GetCenter();
            double radian = Math.PI / 180d;

            double radius = pipeWidth + GetControlCenterWidth() / 2d;

            for (int i = 0; i < Simulation.ControlCenter.Satellites.Count; i++)
            {
                var angleInDegrees = angleStep * i;
                var angleInRadians = angleInDegrees * radian;

                var sX = center.X + radius * Math.Sin(angleInRadians) - pipeWidth / 2;
                var sY = center.Y + radius * Math.Cos(angleInRadians);

                var satellite = new MicrocontrollerView
                {
                    Width = satelliteWidth,
                    Height = satelliteHeight,
                    Index = i,
                    Microcontroller = Simulation.ControlCenter.Satellites[i].Item1,
                };

                var pRadius = radius + satelliteWidth / 2 + satelliteHeight / 2;
                var pX = center.X + pRadius * Math.Sin(angleInRadians) - satelliteWidth / 2;
                var pY = center.Y + pRadius * Math.Cos(angleInRadians) - satelliteHeight / 2;

                satellite.RenderTransformOrigin = new Point(.5, .5);    
                satellite.RenderTransform = new TranslateTransform(pX, pY);
                ContainerView.Children.Add(satellite);

                var pipe = new PipeView
                {
                    Pipe = Simulation.ControlCenter.Satellites[i].Item2,
                    Width = pipeWidth,
                    RenderTransform = new TransformGroup()
                    {
                        Children = {
                            new RotateTransform(-(angleInDegrees + 90)),
                            new TranslateTransform(sX, sY),
                        }
                    },
                    RenderTransformOrigin = new Point(.5, .5)
                };

                ContainerView.Children.Add(pipe);
            }
        }
    }
}
