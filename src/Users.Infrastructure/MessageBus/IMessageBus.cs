using EasyNetQ;

namespace Users.Infrastructure.MessageBus
{
    public interface IMessageBus : IDisposable
    {
        bool IsConnected { get; }
        IAdvancedBus AdvancedBus { get; }
        IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder);
    }
}
