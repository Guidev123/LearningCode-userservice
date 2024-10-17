using MediatR;
using Users.Application.Command.DeleteUser;
using Users.Application.Responses;
using Users.Domain.Entities;
using Users.Domain.Repositories;

namespace Users.API.Endpoints.UserEndpoints
{
    public class DeleteUserEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapDelete("/{id}", HandleAsync).Produces<Response<User?>>().RequireAuthorization();

        public static async Task<IResult> HandleAsync(IMediator mediator,
                                                      IUnitOfWork unitOfWork,
                                                      Guid id)
        {
            var result = await mediator.Send(new DeleteUserCommand(id));

            if (await unitOfWork.Commit() && result.IsSuccess)
                return TypedResults.Ok(result.Message);

            return TypedResults.BadRequest(result.Message);

        }
    }
}
