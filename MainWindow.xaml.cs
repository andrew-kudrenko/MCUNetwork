using System.Windows;
using MCUNetwork.Control;
using MCUNetwork.Models;

namespace MCUNetwork
{
    public partial class MainWindow : Window
    {
        public Simulation Simulation { get; } = new Simulation();
        public Microcontroller Microcontroller { get; } = new(200, 0.6);

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Console.WriteLine("Main Window Init() " + Microcontroller.Memory is null);
            //await Task.Delay(1_000);

            for (int i = 0; i < 5; i++)
            {
                var isSucceed = Microcontroller.Memory.TryReceive(new Message(65));
                Console.WriteLine($"Message has been emitted. Is succeed = {isSucceed}");
            }
        }
    }
}