using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Command.UpdateRoleUser;
using Users.Application.DTOs;
using Users.Application.Responses;
using Users.Infrastructure.MessageBus.Configuration;
using Users.Infrastructure.MessageBus.Messages;

namespace Users.Infrastructure.MessageBus
{
    public class UpdateUserRole : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly BusSettingsConfiguration _busSettings;

        public UpdateUserRole(IServiceProvider serviceProvider, IOptions<BusSettingsConfiguration> busSettings)
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

                if (message is null)
                    throw new Exception();

                var result = await UpdateRole(message);

                if(result.IsSuccess) 
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(_busSettings.Queue, false, consumer);

            return Task.CompletedTask;
        }

        private async Task<Response<GetUserDTO>> UpdateRole(UpdateUserRoleMessage message)
        {
            var clientCommand = new UpdateRoleUserCommand(message.UserId);
            Response<GetUserDTO> sucess;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                sucess = await mediator.Send(clientCommand);
            }
            return sucess;
        }
    }
}
