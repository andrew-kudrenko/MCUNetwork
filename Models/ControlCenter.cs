namespace MCUNetwork.Models
{
    public class ControlCenter
    {
        public Queue<Satellite> ServiceDemanded = new();

        public void DemandService(Satellite satellite) => ServiceDemanded.Enqueue(satellite);

        private IDictionary<Message, int> _receivingMessages = new Dictionary<Message, int>();

        public async Task Service()
        {
            while (ServiceDemanded.TryDequeue(out Satellite satellite))
            {
                await ReceiveMessages(satellite);
            }
        }

        private static async Task ReceiveMessages(Satellite satellite)
        {
            foreach (var message in satellite.Microcontroller.Release())
            {
                await satellite.Pipe.Send(message);
            }
        }
    }
}
