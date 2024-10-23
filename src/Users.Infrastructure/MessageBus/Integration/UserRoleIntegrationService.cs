using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Users.Application.Command.UpdateRoleUser;
using Users.Application.DTOs;
using Users.Application.Responses;

namespace Users.Infrastructure.MessageBus.Integration
{
    public class UserRoleIntegrationService : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public UserRoleIntegrationService(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private void SetResponse()
        {
            _bus.RespondAsync<UserUpdateRoleIntegrationEvent, Response<GetUserDTO>>(async request =>
                await UpdateRole(request));

            _bus.AdvancedBus.Connected += OnConnect!;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponse();
            return Task.CompletedTask;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponse();
        }

        private async Task<Response<GetUserDTO>> UpdateRole(UserUpdateRoleIntegrationEvent message)
        {
            var clientCommand = new UpdateRoleUserCommand(message.UserId);
            Response<GetUserDTO> sucess;

            using(var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                sucess = await mediator.Send(clientCommand);
            }

            return sucess;
        }
    }
}
