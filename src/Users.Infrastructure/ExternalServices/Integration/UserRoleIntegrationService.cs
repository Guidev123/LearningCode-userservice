using EasyNetQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Users.Application.Command.UpdateRoleUser;
using Users.Application.DTOs;
using Users.Application.Responses;
using Users.Infrastructure.Messages;

namespace Users.Infrastructure.ExternalServices.Integration
{
    public class UserRoleIntegrationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BusSettings _busSettings;
        private readonly IBus _bus;

        public UserRoleIntegrationService(IOptions<BusSettings> busSettings, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _busSettings = busSettings.Value;
            _bus = RabbitHutch.CreateBus(_busSettings.Connection, register =>
            {
                register.EnableNewtonsoftJson();
            });
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus.Rpc.RespondAsync<UserUpdateRoleIntegrationEvent, ResponseMessage<GetUserDTO>>
                (async request => new ResponseMessage<GetUserDTO>(await SetUserAsPremium(request)));

            return Task.CompletedTask;
        }

        protected async Task<Response<GetUserDTO>> SetUserAsPremium(UserUpdateRoleIntegrationEvent integrationEvent)
        {
            var userCommand = new UpdateRoleUserCommand(integrationEvent.UserId);

            Response<GetUserDTO>? success = null;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                success = await mediator.Send(userCommand);
            }

            return success;
        }
    }
}
