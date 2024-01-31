using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MCUNetwork.Models;

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

        private readonly List<PipeView> _pipeViews = new();
        private readonly List<MicrocontrollerView> _microcontrollerViews = new();
        private readonly ControlCenterView ControlCenterView = new();
        private Point _center;
        private double _angleFraction;

        public Simulation Simulation
        {
            get => (Simulation) GetValue(SimulationProperty);
            set => SetValue(SimulationProperty, value);
        }

        public SimulationContainer()
        {
            InitializeComponent();
            SizeChanged += (sender, args) => OnResize();
        }

        private void OnResize()
        {
            if (CanBeResized())
            {
                UpdateCenter();

                ControlCenterView.Width = GetControlCenterWidth();

                var microcontrollerWidth = GetMicrocontrollerWidth();
                _microcontrollerViews.ForEach(view => view.Width = microcontrollerWidth);

                var pipeWidth = GetPipeWidth();
                _pipeViews.ForEach(view => view.Width = pipeWidth);

                PositionChildren();
            }
        }

        private bool CanBeResized() => Simulation is not null && IsLoaded && ActualWidth > 0 && ActualHeight > 0;

        private static void OnChangeSimulation(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is SimulationContainer view)
            {
                view.DataContext = view.Simulation;

                if (view.IsLoaded)
                {
                    view.UpdateView();
                } else
                {
                    view.Loaded += (sender, args) => view.UpdateView();
                }
            }
        }

        private void UpdateView()
        {
            ContainerView.Children.Clear();
            _pipeViews.Clear();
            _microcontrollerViews.Clear();

            UpdateCenter();

            AddControlCenter();
            AddPipeViews();
            AddMicrocontrollerViews();

            _angleFraction = 360d / Simulation.Satellites.Count;
            UpdateLayout();

            PositionChildren();
        }

        private void UpdateCenter() => _center = new Point(ActualWidth / 2, ActualHeight / 2);

        private void PositionChildren()
        {
            PositionControlCenter();
            PositionPipes();
            PositionMicrocontrollers();
        }

        private void PositionControlCenter()
        {
            ControlCenterView.RenderTransform = new TranslateTransform() 
            {
                X = _center.X - ControlCenterView.ActualWidth / 2,
                Y = _center.Y - ControlCenterView.ActualHeight / 2
            };
        }

        private int GetControlCenterWidth() => GetValueBetween(120, (int)(ActualWidth / 6), 180);

        private int GetMicrocontrollerWidth() => GetValueBetween(110, (int)(ActualWidth / 6), 150);

        private int GetPipeWidth() => GetValueBetween(90, (int)(ActualWidth / 10), 200);

        private static int GetValueBetween(int min, int actual, int max) => Math.Min(max, Math.Max(actual, min));

        private void AddControlCenter()
        {
            ControlCenterView.ControlCenter = Simulation.ControlCenter;
            ControlCenterView.Width = GetControlCenterWidth();
            ControlCenterView.RenderTransformOrigin = new(.5, .5);

            ContainerView.Children.Add(ControlCenterView);
        }

        private void AddMicrocontrollerViews()
        {
            for (int i = 0; i < Simulation.Satellites.Count; i++)
            {
                var microcontroller = new MicrocontrollerView
                {
                    Index = i + 1,
                    Microcontroller = Simulation.Satellites[i].Microcontroller,
                    Pipe = _pipeViews[i].Pipe,
                    Width = GetMicrocontrollerWidth(),
                };

                _microcontrollerViews.Add(microcontroller);
                ContainerView.Children.Add(microcontroller);
            }
        }

        private void AddPipeViews()
        {
            for (int i = 0; i < Simulation.Satellites.Count; i++)
            {
                var pipe = new PipeView { 
                    Pipe = Simulation.Satellites[i].Pipe,
                    Width = GetPipeWidth(),
                };

                _pipeViews.Add(pipe);
                ContainerView.Children.Add(pipe);
            }
        }

        private void PositionPipes()
        {
            var transformOrigin = new Point(.5, .5);
            double radius = GetPipeRadius();

            for (int i = 0; i < _pipeViews.Count; i++)
            {
                var pipe = _pipeViews[i];

                pipe.RenderTransformOrigin = transformOrigin;
             
                var angle = _angleFraction * i;
                var translate = GetPipeTranslate(angle, radius);
                
                pipe.RenderTransform = new TransformGroup()
                {
                    Children = new() 
                    {
                        new RotateTransform(-(angle + 90)),
                        new TranslateTransform(translate.X, translate.Y),
                    },
                };
            }
        }

        private void PositionMicrocontrollers()
        {
            var transformOrigin = new Point(.5, .5);
            double radius = GetMicrocontrollerRadius();

            for (int i = 0; i < _microcontrollerViews.Count; i++)
            {
                var angle = _angleFraction * i;
                var translate = GetMicrocontrollerTranslate(angle, radius);

                var satellite = _microcontrollerViews[i];

                satellite.RenderTransformOrigin = transformOrigin;    
                satellite.RenderTransform = new TranslateTransform(translate.X, translate.Y);
            }
        }

        private Point GetMicrocontrollerTranslate(double angle, double radius) => 
            GetTranslate(angle, radius, _microcontrollerViews[0].ActualWidth, _microcontrollerViews[0].ActualHeight);

        private Point GetPipeTranslate(double angle, double radius) => 
            GetTranslate(angle, radius, _pipeViews[0].ActualWidth, _pipeViews[0].ActualHeight);

        private Point GetTranslate(double angle, double radius, double width, double height)
        {
            var radiansAngle = AsRadians(angle);

            return new Point(
                _center.X + radius * Math.Sin(radiansAngle) - width / 2,
                _center.Y + radius * Math.Cos(radiansAngle) - height / 2
            );
        }

        private double GetMicrocontrollerRadius() => 
            _microcontrollerViews[0].ActualWidth / 2 + _pipeViews[0].ActualWidth + ControlCenterView.ActualWidth / 2;

        private double GetPipeRadius() => 
            _pipeViews[0].ActualWidth / 2 + ControlCenterView.ActualWidth / 2;

        private static double AsRadians(double angle) => Math.PI / 180d * angle;
    }
}
