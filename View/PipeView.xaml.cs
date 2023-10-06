using MCUNetwork.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MCUNetwork.View
{
    public partial class PipeView : UserControl
    {
        public readonly static DependencyProperty PipeProperty = DependencyProperty.Register(
            "Pipe",
            typeof(Pipe),
            typeof(PipeView), 
            new PropertyMetadata(null, new(OnChangePipe))
            );

        public Pipe Pipe
        {
            get => (Pipe)GetValue(PipeProperty);
            set => SetValue(PipeProperty, value);
        }

        public PipeView()
        {
            InitializeComponent();
        }

        private void MarkAsBusy() => LineView.Background = Brushes.Cyan;

        private void MarkAsFree() => LineView.Background = Brushes.Black;

        private static void OnChangePipe(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is PipeView view)
            {
                view.Pipe.OnSent += _ => view.MarkAsBusy();
                view.Pipe.OnReceived += _ => view.MarkAsFree();
            }
        }
    }
}
