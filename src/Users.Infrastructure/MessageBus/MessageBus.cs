using EasyNetQ;
using Polly;
using RabbitMQ.Client.Exceptions;

namespace Users.Infrastructure.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private IBus _bus;
        private readonly string _settings;
        private IAdvancedBus _advancedBus;

        public MessageBus(string settings)
        {
            _settings = settings;
            TryConnect();
        }

        public bool IsConnected => _bus?.Advanced.IsConnected ?? false;
        public IAdvancedBus AdvancedBus => _bus.Advanced;

        public IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
        {
            TryConnect();
            var disposable = _bus.Rpc.RespondAsync(responder);
            return disposable.GetAwaiter().GetResult();
        }

        private void TryConnect()
        {
            if (IsConnected) return;

            var policy = Policy.Handle<EasyNetQException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));

            policy.Execute(() =>
            {
                _bus = RabbitHutch.CreateBus(_settings, register =>
                {
                    register.EnableNewtonsoftJson();
                });
                _advancedBus = _bus.Advanced;
                _advancedBus.Disconnected += OnDisconnect;
                _bus = RabbitHutch.CreateBus(_settings);
            });
        }

        private void OnDisconnect(object x, EventArgs y)
        {
            var policy = Policy.Handle<EasyNetQException>()
                .Or<BrokerUnreachableException>()
                .RetryForever();

            policy.Execute(TryConnect);
        }

        public void Dispose() => _bus?.Dispose();
    }
}
