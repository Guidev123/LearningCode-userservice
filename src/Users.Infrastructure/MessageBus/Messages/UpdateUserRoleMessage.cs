namespace Users.Infrastructure.MessageBus.Messages
{
    public class UpdateUserRoleMessage
    {
        public UpdateUserRoleMessage(Guid userId, bool isPaid)
        {
            UserId = userId;
            IsPaid = isPaid;
        }

        public Guid UserId { get; private set; }
        public bool IsPaid { get; private set; }
    }
}
