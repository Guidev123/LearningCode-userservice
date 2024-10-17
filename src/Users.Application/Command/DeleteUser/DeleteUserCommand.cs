using MediatR;
using Users.Application.Responses;
using Users.Domain.Entities;

namespace Users.Application.Command.DeleteUser
{
    public class DeleteUserCommand(Guid id) : IRequest<Response<User?>>
    {
        public Guid Id { get; } = id;
    }
}
