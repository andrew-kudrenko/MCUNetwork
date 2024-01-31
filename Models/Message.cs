namespace MCUNetwork.Models
{
    public class Message
    {
        public int Size { get; set; }

        public Message(int size) => Size = size;
    }
}
