using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Repositories;

namespace Users.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork(UserDbContext context) : IUnitOfWork
    {
        private readonly UserDbContext _context = context;
        public async Task<bool> Commit()
        {
            var ret = await _context.SaveChangesAsync();
            if (ret > 0)
                return true;

            return false;
        }

        public async Task<bool> Rollback() => await Task.FromResult(true);
    }
}
