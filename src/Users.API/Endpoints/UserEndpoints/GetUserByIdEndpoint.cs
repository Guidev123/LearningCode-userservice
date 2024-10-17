using MediatR;
using Users.Application.Queries.GetUserById;
using Users.Application.Responses;
using Users.Domain.Entities;

namespace Users.API.Endpoints.UserEndpoints
{
    public class GetUserByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapGet("/{id}", HandleAsync).Produces<Response<User?>>().RequireAuthorization();

        public static async Task<IResult> HandleAsync(IMediator mediator,
                                                      Guid id)
        {
            var result = await mediator.Send(new GetUserByIdQuery(id));

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }
    }
}
