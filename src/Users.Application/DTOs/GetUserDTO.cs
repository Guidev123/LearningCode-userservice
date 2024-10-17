using Users.Domain.Entities;

namespace Users.Application.DTOs
{
    public class GetUserDTO(Guid id, string fullName, string phone, string email)
    {
        public Guid Id { get; private set; } = id;
        public string FullName { get; private set; } = fullName;
        public string Phone { get; private set; } = phone;
        public string Email { get; private set; } = email;

        public static GetUserDTO MapFromEntity(User user) =>
            new(user.Id, user.FullName, user.Phone, user.Email.Address);
    }
}
