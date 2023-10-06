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
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(MicrocontrollerView));

        public Microcontroller Microcontroller {
            get => (Microcontroller) GetValue(MicrocontrollerProperty); 
            set => SetValue(MicrocontrollerProperty, value);
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
                var microcontroller = (Microcontroller) args.NewValue;

                if (view.Messages.Count > 0)
                {
                    view.Messages.Clear();
                }

                view.SizeText.Text = microcontroller.Memory.Size.ToString();

                microcontroller.Memory.OnMessageReceived += message =>
                {
                    view.Messages.Add(message);
                    UpdateView(view, microcontroller);
                };

                microcontroller.Memory.OnServiceDemanded += () => 
                {
                    view.MemoryBar.Foreground = Brushes.Yellow;
                };

                microcontroller.Memory.OnMessageIgnored += _ =>
                {
                    view.MemoryBar.Foreground = Brushes.Red;
                };

                microcontroller.Memory.OnServiceIsDone += () =>
                {
                    view.MemoryBar.Foreground = Brushes.Green;
                    view.Messages.Clear();

                    UpdateView(view, microcontroller);
                };
            }
        }

        private static void UpdateView(MicrocontrollerView view, Microcontroller microcontroller)
        {
            view.MemoryBar.Value = microcontroller.Memory.BusyAsPercents;
            view.MessageCount.Text = microcontroller.Memory.MessagesCount.ToString();
            view.BusyPercentText.Text = microcontroller.Memory.BusyAsPercents.ToString("N2");
            view.BusyMbText.Text = microcontroller.Memory.Busy.ToString();
        }
    }
}
