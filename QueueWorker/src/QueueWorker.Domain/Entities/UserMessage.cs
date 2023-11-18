namespace QueueWorker.Domain.Entities
{
    public class UserMessage(string userName, string messageContent)
    {
        public string MessageId { get; } = Guid.NewGuid().ToString();

        public string UserName { get; set; } = userName;

        public string MessageContent { get; set; } = messageContent;

        public string GetUserMessage()
        {
            return string.Format("Reading message {0}. User : {1}, Message : {2}", MessageId, UserName, MessageContent);            
        }
    }
}
