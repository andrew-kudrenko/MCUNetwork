namespace MCUNetwork.Models
{
    public class StaticMessageGenerator : IMessageGenerator
    {
        public int Size { get; set; }

        public Message Generate() => new(Size);
    }
}
