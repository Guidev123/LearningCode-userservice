using Users.Domain.Entities;

namespace Users.Application.Authentication
{
    public interface IAuthenticationService
    {
        string GenerateJwtToken(User user);
        string ComputeSha256Hash(string password);
    }
}
