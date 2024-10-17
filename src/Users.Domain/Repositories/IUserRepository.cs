using Users.Domain.Entities;

namespace Users.Domain.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> UserAlreadyExists(User user);
        public void CreateUserAsync(User user);
        public void UpdateUser(User user);
        public Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAndPasswordAsync(string email, string passwordHash);
    }
}
