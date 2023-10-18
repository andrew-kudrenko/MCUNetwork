namespace MCUNetwork.Models
{
    public class DynamicMessageGenerator : IMessageGenerator
    {
        private readonly Random _random = new();
        private readonly Tuple<int, int> _sizeRange;

        public DynamicMessageGenerator(Tuple<int, int> sizeRange) => _sizeRange = sizeRange;

        public Message Generate() => new(_random.Next(_sizeRange.Item1, _sizeRange.Item2));
    }
}
