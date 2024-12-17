using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Users.Application.Command.UpdateRoleUser;
using Users.Application.MessageBus.Configuration;
using Users.Application.MessageBus.Messages;
using Users.Application.Responses;
using Users.Domain.Repositories;

namespace Users.Application.BackgroundServices
{
    public class UpdateUserRoleService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly BusSettingsConfiguration _busSettings;

        public UpdateUserRoleService(IServiceProvider serviceProvider, IOptions<BusSettingsConfiguration> busSettings)
        {
            _busSettings = busSettings.Value;
            _serviceProvider = serviceProvider;
            var connectionFactory = new ConnectionFactory
            {
                HostName = _busSettings.Hostname,
            };
            _connection = connectionFactory.CreateConnection(_busSettings.ClientProvidedName);
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_busSettings.Exchange, _busSettings.QueueType, true);
            _channel.QueueDeclare(_busSettings.Queue, true, false, false, null);
            _channel.QueueBind(_busSettings.Queue, _busSettings.Exchange, _busSettings.RoutingKey);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var byteArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(byteArray);
                var message = JsonConvert.DeserializeObject<UpdateUserRoleMessage>(contentString);

                var result = await UpdateRole(message!);
                if (result.IsSuccess)
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(_busSettings.Queue, false, consumer);

            return Task.CompletedTask;
        }

        private async Task<Response<string?>> UpdateRole(UpdateUserRoleMessage message)
        {
            var clientCommand = new UpdateRoleUserCommand(message!.UserId);
            Response<string?> sucess;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                sucess = await mediator.Send(clientCommand);

                if (sucess.IsSuccess) await unitOfWork.Commit();
            }
            return sucess;
        }
    }
}
