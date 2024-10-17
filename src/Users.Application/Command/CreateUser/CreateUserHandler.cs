using MediatR;
using Users.Application.Authentication;
using Users.Application.DTOs;
using Users.Application.Responses;
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

            if (!validationResult.IsValid)
                return new Response<GetUserDTO?>(null, 400, "Error: Something is wrong");

            if (await _userRepository.UserAlreadyExists(user))
                return new Response<GetUserDTO?>(null, 400, "Error: User already exists");

            user.CryptographyPassword(_authenticationService.ComputeSha256Hash(user.Password));
            _userRepository.CreateUserAsync(user);

            var result = GetUserDTO.MapFromEntity(user);

            return new Response<GetUserDTO?>(result, 201);
        }
    }
}
