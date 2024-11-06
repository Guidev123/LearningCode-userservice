using MediatR;
using System.Security.Claims;
using Users.Application.Command.RefreshTokenUser;
using Users.Application.Responses;

namespace Users.API.Endpoints.UserEndpoints
{
    public class RefreshTokenEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPost("/refresh-token", HandleAsync).Produces<Response<string?>> ();

        public static async Task<IResult> HandleAsync(IMediator mediator,
                                                      ClaimsPrincipal user,
                                                      RefreshTokenUserCommand command)
        {
            var userName = user.Identity!.Name;
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var result = await mediator.Send(new RefreshTokenUserCommand(command.Token, command.RefreshToken,
                                                                         userName!, Guid.Parse(userIdClaim!.Value)));
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
