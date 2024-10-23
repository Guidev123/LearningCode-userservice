namespace Users.Infrastructure.MessageBus.Messages
{
    public class Event : Message
    {
        public DateTime Timestamp { get; private set; }

        public Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
