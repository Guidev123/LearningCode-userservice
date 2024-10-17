using MediatR;
using Users.Application.DTOs;
using Users.Application.Responses;
using Users.Application.Responses.Messages;
using Users.Domain.Repositories;

namespace Users.Application.Queries.GetUserById
{
    public class GetUserByIdHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, Response<GetUserDTO?>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<Response<GetUserDTO?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id);
            if(user is null)
                return new Response<GetUserDTO?>(null, 404, ResponseMessages.USER_NOT_FOUND.GetDescription());

            var result = GetUserDTO.MapFromEntity(user);

            return new Response<GetUserDTO?>(result, 200, ResponseMessages.USER_RETRIEVED_SUCCESS.GetDescription());
        }
    }
}
