namespace MCUNetwork.Models
{
    public class ExternalDataSource
    {
        public IMessageGenerator MessageGenerator { get; set; }

        public ExternalDataSource(IMessageGenerator generator) => MessageGenerator = generator;

        public Message Send() => MessageGenerator.Generate();
    }
}
