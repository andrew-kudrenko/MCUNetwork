using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MCUNetwork.Models;

namespace MCUNetwork.View
{
    public partial class MicrocontrollerView : UserControl
    {
        public static readonly DependencyProperty MicrocontrollerProperty = DependencyProperty.Register(
            "Microcontroller",
            typeof(Microcontroller),
            typeof(MicrocontrollerView),
            new PropertyMetadata(null, new(OnChangeMicrocontroller))
        );

        public static readonly DependencyProperty PipeProperty = DependencyProperty.Register(
            "Pipe",
            typeof(Pipe),
            typeof(MicrocontrollerView),
            new PropertyMetadata(null, new(OnChangePipe))
        );

        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(MicrocontrollerView));

        public Microcontroller Microcontroller {
            get => (Microcontroller)GetValue(MicrocontrollerProperty);
            set => SetValue(MicrocontrollerProperty, value);
        }

        public Pipe Pipe
        {
            get => (Pipe)GetValue(PipeProperty);
            set => SetValue(PipeProperty, value);
        }

        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public readonly ObservableCollection<Models.Message> Messages = new();

        public MicrocontrollerView()
        {
            InitializeComponent();
        }

        private static void OnChangeMicrocontroller(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is MicrocontrollerView view)
            {
                var microcontroller = (Microcontroller)args.NewValue;

                if (view.Messages.Count > 0)
                {
                    view.Messages.Clear();
                }

                view.SizeText.Text = microcontroller.Size.ToString();

                microcontroller.OnMessageReceived += message =>
                {
                    view.Messages.Add(message);
                    UpdateView(view, microcontroller);
                };
                microcontroller.OnServiceDemanded += () => view.MemoryBar.Foreground = Brushes.Yellow;
                microcontroller.OnMessageIgnored += _ => view.MemoryBar.Foreground = Brushes.Red;
                microcontroller.OnServiceIsDone += () =>
                {
                    view.MemoryBar.Foreground = Brushes.Green;
                    view.Messages.Clear();

                    UpdateView(view, microcontroller);
                };
            }
        }
        private static void OnChangePipe(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is MicrocontrollerView view)
            {
                var pipe = (Pipe) args.NewValue;
                view.Pipe.OnProgress += _ => UpdateView(view, view.Microcontroller);
            }
        }

        private static void UpdateView(MicrocontrollerView view, Microcontroller microcontroller)
        {
            view.MemoryBar.Value = microcontroller.BusyAsPercents;
            view.MessageCount.Text = microcontroller.CurrentMessagesCount.ToString();
            view.BusyPercentText.Text = microcontroller.BusyAsPercents.ToString();
            view.BusyMbText.Text = microcontroller.Busy.ToString();
        }
    }
}
