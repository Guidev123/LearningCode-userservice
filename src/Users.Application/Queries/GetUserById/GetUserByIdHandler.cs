using MediatR;
using Users.Application.DTOs;
using Users.Application.Responses;
using Users.Domain.Entities;
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
                return new Response<GetUserDTO?>(null, 404, "Error: User not found");

            var result = GetUserDTO.MapFromEntity(user);

            return new Response<GetUserDTO?>(result, 200);
        }
    }
}
