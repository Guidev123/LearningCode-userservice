namespace Users.Infrastructure.ExternalServices.Integration
{
    public class UserUpdateRoleIntegrationEvent(bool orderIsPaid, Guid userId) : IntegrationEvent
    {
        public Guid UserId { get; private set; } = userId;
        public bool OrderIsPaid { get; private set; } = orderIsPaid;
    }
}
