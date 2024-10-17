using MediatR;
using Users.Application.Command.LoginUser;
using Users.Application.Responses;

namespace Users.API.Endpoints.UserEndpoints
{
    public class LoginUserEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPost("/login", HandleAsync).Produces<Response<string?>>();

        public static async Task<IResult> HandleAsync(IMediator mediator,
                                                      LoginUserCommand command)
        {
            var result = await mediator.Send(command);

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }
    }
}
