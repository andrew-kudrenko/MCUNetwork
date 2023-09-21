using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MCUNetwork.Models;

namespace MCUNetwork.View
{
    public partial class MicrocontrollerView : UserControl
    {
        public static readonly DependencyProperty MicrocontrollerProperty;

        public Microcontroller Microcontroller { 
            get => (Microcontroller) GetValue(MicrocontrollerProperty); 
            set => SetMicrocontroller(value); 
        }

        public ObservableCollection<Models.Message> Messages { get; set; } = new();

        static MicrocontrollerView()
        {
            MicrocontrollerProperty = DependencyProperty.Register(
                "Microcontroller", 
                typeof(Microcontroller), 
                typeof(MicrocontrollerView)
            );
        }

        public MicrocontrollerView()
        {
            InitializeComponent();
            MessageList.ItemsSource = Messages;
        }

        private void SetMicrocontroller(Microcontroller microcontroller)
        {
            Messages.Clear();
            
            SetValue(MicrocontrollerProperty, microcontroller);

            microcontroller.Memory.OnMessageReceived += Messages.Add;
            microcontroller.Memory.OnMessageIgnored += Messages.Add;

        }
    }
}
