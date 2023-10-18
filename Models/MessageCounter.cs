namespace MCUNetwork.Models
{
    public class MessageCounter
    {
        public int Ignored { get; private set; }
        public int Received { get; private set; }
        public int Total { get; private set; }

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
