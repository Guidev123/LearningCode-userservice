using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.DTOs;
using Users.Application.Responses;
using Users.Application.Responses.Messages;
using Users.Domain.Repositories;

namespace Users.Application.Command.UpdateRoleUser
{
    public class UpdateRoleUserHandler(IUserRepository userRepository) : IRequestHandler<UpdateRoleUserCommand, Response<string?>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<Response<string?>> Handle(UpdateRoleUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if(user is null)
                return new Response<string?>(null, 404, ResponseMessages.USER_NOT_FOUND.GetDescription());

            user.SetUserAsPremium();
            _userRepository.UpdateUser(user);

            return new Response<string?>(string.Empty, 200, ResponseMessages.USER_RETRIEVED_SUCCESS.GetDescription());
        }
    }
}
