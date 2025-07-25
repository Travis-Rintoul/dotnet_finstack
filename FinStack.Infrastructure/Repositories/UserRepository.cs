using FinStack.Domain.Entities;
using FinStack.Infrastructure.Data;
using FinStack.Domain.Repositories;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

namespace FinStack.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public async Task<Option<User>> GetByIdAsync(Guid userId)
            => await _context.Users.FindAsync(userId);

        public async Task<IEnumerable<User>> GetUsersAsync()
            => await _context.Users.ToListAsync();

        public async Task<Result<Guid>> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Guid;
        }

        public async Task<Result<Guid>> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user.Guid;
        }
    }
}

