using MediatR;
using Users.Application.DTOs;
using Users.Application.Responses;
using Users.Domain.Entities;

namespace Users.Application.Queries.GetUserById
{
    public class GetUserByIdQuery(Guid id) : IRequest<Response<GetUserDTO?>>
    {
        public Guid Id { get; } = id;
    }
}
