namespace MCUNetwork.Models
{
    internal interface ISimulation
    {
        public void Run();
        public void Pause();
        public void Stop();
        public void Reset();
    }
}
