namespace MCUNetwork.Models
{
    public class MessageCounter
    {
        public int Ignored { get; private set; }
        public int Received { get; private set; }
        public int Total { get; private set; }

        public double IgnoredAsPercents { get => Total > 0 ? (double)Ignored / Total * 100 : 0; }

        public double ReceivedAsPercents { get => Total > 0 ? (double)Received / Total * 100 : 0; }

        public void Receive()
        {
            Received++;
            Total++;
        }

        public void Ignore()
        {
            Ignored++;
            Total++;
        }
    }
}
