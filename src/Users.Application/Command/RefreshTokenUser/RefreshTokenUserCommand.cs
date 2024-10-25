using MediatR;
using System.Text.Json.Serialization;
using Users.Application.DTOs;
using Users.Application.Responses;

namespace Users.Application.Command.RefreshTokenUser
{
    public class RefreshTokenUserCommand : IRequest<Response<GetUserTokensDTO?>>
    {
        public RefreshTokenUserCommand(string token, string refreshToken, string name, Guid userId)
        {
            Token = token;
            RefreshToken = refreshToken;
            Name = name;
            UserId = userId;
        }
        [JsonIgnore]
        public Guid UserId { get; private set; }
        [JsonIgnore]
        public string Name { get; private set; }
        public string Token { get; private set; }
        public string RefreshToken { get; private set; }
    }
}
