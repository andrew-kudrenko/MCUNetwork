using MCUNetwork.Models;
using System.Windows;
using System.Windows.Controls;

namespace MCUNetwork.View
{
    public partial class ControlCenterView : UserControl
    {
        public static readonly DependencyProperty ControlCenterProperty = DependencyProperty.Register(
            "ControlCenter",
            typeof(ControlCenter),
            typeof(ControlCenterView),
            new PropertyMetadata(null, new(OnChangeControlCenter))
        );

        public ControlCenter ControlCenter
        {
            get => (ControlCenter) GetValue(ControlCenterProperty);
            set => SetValue(ControlCenterProperty, value);
        }

        public ControlCenterView()
        {
            InitializeComponent();
        }

        private static void OnChangeControlCenter(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is ControlCenterView view)
            {
                var value = (ControlCenter)args.NewValue;
                value.OnReceiveMessage += _ => view.ReceivedMessages.Text = value.ReceivedMessages.ToString();
            }
        }
    }
}
