using Users.Domain.Entities;

namespace Users.Application.DTOs
{
    public class GetUserDTO(Guid id, string fullName, string phone, string email, string role, string token)
    {
        public Guid Id { get; private set; } = id;
        public string FullName { get; private set; } = fullName;
        public string Phone { get; private set; } = phone;
        public string Email { get; private set; } = email;
        public string Role { get; private set; } = role;
        public string Token { get; private set; } = token;
        public static GetUserDTO MapFromEntity(User user, string? token = null) =>
            new(user.Id, user.FullName, user.Phone, user.Email.Address, user.UserType.ToString(), token ?? string.Empty);
    }
}
