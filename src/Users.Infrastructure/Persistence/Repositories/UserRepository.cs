﻿using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Repositories;

namespace Users.Infrastructure.Persistence.Repositories
{
    public class UserRepository(UserDbContext context) : IUserRepository
    {
        private readonly UserDbContext _context = context;

        public void CreateUserAsync(User customer) => _context.Add(customer);

        public void UpdateUser(User user) => _context.Users.Update(user);

        public async Task<bool> UserAlreadyExists(User user) =>
             await _context.Users.AsNoTracking().AnyAsync(x => x.Email.Address == user.Email.Address
                                                                        || x.Document.Number == user.Document.Number 
                                                                        || x.Phone == user.Phone);

        public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string passwordHash) =>
            await _context.Users.SingleOrDefaultAsync(x => x.Email.Address == email && x.Password == passwordHash);

        public async Task<User?> GetUserByIdAsync(Guid id) =>
            await _context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

    }
}
