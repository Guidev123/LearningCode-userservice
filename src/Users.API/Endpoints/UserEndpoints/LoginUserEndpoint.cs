using MediatR;
using Users.Application.Command.LoginUser;
using Users.Application.Responses;
using Users.Domain.Repositories;

namespace Users.API.Endpoints.UserEndpoints
{
    public class LoginUserEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPost("/login", HandleAsync).Produces<Response<string?>>();

        public static async Task<IResult> HandleAsync(IMediator mediator,
                                                      IUnitOfWork uow,
                                                      LoginUserCommand command)
        {
            var result = await mediator.Send(command);

            return result.IsSuccess && await uow.Commit()
                ? TypedResults.Ok(result) 
                : TypedResults.BadRequest(result);
        }
    }
}
