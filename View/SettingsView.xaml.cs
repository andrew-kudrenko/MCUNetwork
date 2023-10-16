using System.Windows;
using System.Windows.Controls;
using MCUNetwork.Models;

namespace MCUNetwork.View
{
    public partial class SettingsView : UserControl
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(StaticSimulationOptions),
            typeof(SettingsView), 
            new PropertyMetadata(null)
        );

        public StaticSimulationOptions Value
        {
            get => (StaticSimulationOptions)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
