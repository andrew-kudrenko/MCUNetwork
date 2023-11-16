namespace MCUNetwork.Models
{
    public class ControlCenter
    {
        public Queue<Satellite> ServiceDemanded = new();

        public void DemandService(Satellite satellite) => ServiceDemanded.Enqueue(satellite);

        public async Task Service()
        {
            while (ServiceDemanded.TryDequeue(out Satellite satellite))
            {
                await SendMessages(satellite);
            }
        }

        private static async Task SendMessages(Satellite satellite)
        {
            foreach (var message in satellite.Microcontroller.Release())
            {
                await satellite.Pipe.Send(message);
            }
        }
    }
}
