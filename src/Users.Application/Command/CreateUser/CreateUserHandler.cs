using MediatR;
using Users.Application.Authentication;
using Users.Application.DTOs;
using Users.Application.Responses;
using Users.Application.Responses.Messages;
using Users.Domain.Entities;
using Users.Domain.Entities.Validations;
using Users.Domain.Repositories;

namespace Users.Application.Command.CreateUser
{
    public class CreateUserHandler(IUserRepository userRepository,
                                   IAuthenticationService authenticationService)
                                 : IRequestHandler<CreateUserCommand, Response<GetUserDTO?>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAuthenticationService _authenticationService = authenticationService;
        public async Task<Response<GetUserDTO?>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.MapToEntity();
            var validationResult = new UserValidation().Validate(user);

            if (!user.Document.Validate())
                return new Response<GetUserDTO?>(null, 400, ResponseMessages.INVALID_DOCUMENT.GetDescription());

            if (!validationResult.IsValid)
                return new Response<GetUserDTO?>(null, 400, ResponseMessages.USER_CREATION_FAILED.GetDescription());

            if (await _userRepository.UserAlreadyExists(user))
                return new Response<GetUserDTO?>(null, 400, ResponseMessages.USER_ALREADY_EXISTS.GetDescription());

            user.CryptographyPassword(_authenticationService.ComputeSha256Hash(user.Password));
            _userRepository.CreateUserAsync(user);

            var token = _authenticationService.GenerateJwtToken(user);

            var result = GetUserDTO.MapFromEntity(user, token);

            return new Response<GetUserDTO?>(result, 201, ResponseMessages.USER_CREATED_SUCCESS.GetDescription());
        }
    }
}
