using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
using Users.Infrastructure.MessageBus.Messages;

namespace Users.Infrastructure.MessageBus
{
    public class UpdateUserRole : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string QUEUE = "order-service/set-user-premium";
        private const string EXCHANGE = "order-service";
        private const string ROUTING_KEY = "set-user-premium";

        public UpdateUserRole(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
            };
            _connection = connectionFactory.CreateConnection("order-service-set-user-premium");
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(EXCHANGE, "topic", true);
            _channel.QueueDeclare(QUEUE, true, false, false, null);
            _channel.QueueBind(QUEUE, "order-service", ROUTING_KEY);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var byteArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(byteArray);
                var message = JsonConvert.DeserializeObject<UpdateUserRoleIntegrationEvent>(contentString);

                if (message is null)
                    throw new Exception();

                var result = await UpdateRole(message);

                if(result.IsSuccess) 
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE, false, consumer);

            return Task.CompletedTask;
        }

        private async Task<Response<GetUserDTO>> UpdateRole(UpdateUserRoleIntegrationEvent message)
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
