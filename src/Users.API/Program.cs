using Users.API.Configurations;
using Users.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.AddConfigurations();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<SecurityConfiguration>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapEndpoints();

app.Run();
