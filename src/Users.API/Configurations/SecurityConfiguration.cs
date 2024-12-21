using Microsoft.Extensions.Options;

namespace Users.API.Configurations
{
    public class SecurityConfiguration(IOptions<SecurityKey> options, RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private readonly SecurityKey _config = options.Value;

        public async Task InvokeAsync(HttpContext context)
        {

            if (!context.Request.Headers.TryGetValue("AcessSecretKey", out var providedSecretKey) ||
                providedSecretKey != _config.Key)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied: Invalid or missing AcessSecretKey");
                return;
            }

            await _next(context);
        }
    }

    public class SecurityKey
    {
        public string Key { get; set; } = string.Empty;
    }
}
