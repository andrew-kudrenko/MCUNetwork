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

        public Microcontroller Microcontroller {
            get => (Microcontroller) GetValue(MicrocontrollerProperty); 
            set => SetValue(MicrocontrollerProperty, value);
        }

        public readonly ObservableCollection<Models.Message> Messages = new();

        public MicrocontrollerView()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            MessageList.ItemsSource = Messages;
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

                microcontroller.Memory.OnMessageReceived += message =>
                {
                    view.Messages.Add(message);
                    
                    view.MemoryBar.Value = microcontroller.Memory.BusyAsPercents;

                    view.MessageCount.Text = microcontroller.Memory.MessagesCount.ToString();
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
                };
            }
        }
    }
}
