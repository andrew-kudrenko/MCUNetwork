namespace MCUNetwork.Models
{
    public class StaticMessageGenerator : IMessageGenerator
    {
        private readonly int _size;

        public StaticMessageGenerator(int size) => _size = size;

        public Message Generate() => new(_size);
    }
}
