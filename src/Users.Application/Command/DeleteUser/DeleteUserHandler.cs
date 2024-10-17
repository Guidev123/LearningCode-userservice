using MediatR;
using Users.Application.Responses;
using Users.Application.Responses.Messages;
using Users.Domain.Entities;
using Users.Domain.Repositories;

namespace Users.Application.Command.DeleteUser
{
    public class DeleteUserHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Response<User?>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<Response<User?>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id);
            if (user is null)
                return new Response<User?>(null, 404, ResponseMessages.USER_NOT_FOUND.GetDescription());

            user.SetEntityAsDeleted();
            _userRepository.UpdateUser(user);

            return new Response<User?>(null, 204, ResponseMessages.USER_DELETED_SUCCESS.GetDescription()); 
        }
    }
}
