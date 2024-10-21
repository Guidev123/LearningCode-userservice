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
                                                      IUnitOfWork uow,
                                                      CreateUserCommand command)
        {
            var result = await mediator.Send(command);

            return result.IsSuccess && await uow.Commit()
                ? TypedResults.Created($"api/users/{result.Data?.Id}", result)
                : TypedResults.BadRequest(result);

        }
    }
}
