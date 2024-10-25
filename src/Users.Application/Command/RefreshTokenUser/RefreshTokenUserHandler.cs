using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Authentication;
using Users.Application.DTOs;
using Users.Application.Responses;
using Users.Application.Responses.Messages;
using Users.Domain.Repositories;

namespace Users.Application.Command.RefreshTokenUser
{
    public class RefreshTokenUserHandler(IAuthenticationService authenticationService, IUserRepository userRepository)
               : IRequestHandler<RefreshTokenUserCommand, Response<GetUserTokensDTO?>>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<Response<GetUserTokensDTO?>> Handle(RefreshTokenUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user is null)
                return new Response<GetUserTokensDTO?>(null, 404, ResponseMessages.USER_NOT_FOUND.GetDescription());

            var newJwt = _authenticationService.GenerateJwtToken(user);
            var newRefreshToken = _authenticationService.GenerateRefreshToken();
            user.UpdateLastLoginDate(); 

            return new Response<GetUserTokensDTO?>(new(newJwt, newRefreshToken), 200, ResponseMessages.REFRESH_TOKEN_GENERATED.GetDescription());
        }
    }
}
