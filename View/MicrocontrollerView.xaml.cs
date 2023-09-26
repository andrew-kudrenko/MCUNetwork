using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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

            MessageList.ItemsSource = Messages;
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

                microcontroller.Memory.OnMessageReceived += m =>
                {
                    view.Messages.Add(m);
                    view.FreeSpaceBar.Value = microcontroller.Memory.BusyAsPercents;
                };
            }
        }
    }
}
