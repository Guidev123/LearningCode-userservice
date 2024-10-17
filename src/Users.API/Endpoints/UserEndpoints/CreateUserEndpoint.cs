
using MediatR;
using Users.Application.Command.CreateUser;
using Users.Application.Responses;
using Users.Domain.Entities;
using Users.Domain.Repositories;

namespace Users.API.Endpoints.UserEndpoints
{
    public class CreateUserEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPost("/register", HandleAsync).Produces<Response<User?>>();

        public static async Task<IResult> HandleAsync(IMediator mediator,
                                                      IUnitOfWork unitOfWork,
                                                      CreateUserCommand command)
        {
            var result = await mediator.Send(command);

            if (await unitOfWork.Commit() && result.IsSuccess)
                return TypedResults.Created($"api/users/{result.Data?.Id}", result);

            return TypedResults.BadRequest(result);

        }
    }
}
