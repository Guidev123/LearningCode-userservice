using Users.API.Endpoints.UserEndpoints;

namespace Users.API.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app
                .MapGroup("");

            endpoints.MapGroup("api/users")
                .WithTags("Users")
                .MapEndpoint<CreateUserEndpoint>()
                .MapEndpoint<DeleteUserEndpoint>()
                .MapEndpoint<GetUserByIdEndpoint>()
                .MapEndpoint<RefreshTokenEndpoint>()
                .MapEndpoint<LoginUserEndpoint>();

        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
            where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);
            return app;
        }
    }
}
