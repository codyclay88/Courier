namespace Courier.AspNetCoreSample.MessageTypes
{
    public class TextMessage : IMessage
    {
        public TextMessage(string contents)
        {
            Contents = contents;
        }

        public string Contents { get; }
    }
}