using FinStack.Common;
using FinStack.Domain.Entities;
using FinStack.Infrastructure.Data;
using FinStack.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using static FinStack.Common.Result;

namespace FinStack.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<Option<User>> GetByIdAsync(Guid userId)
            => await context.Users.FindAsync(userId);

        public async Task<IEnumerable<User>> GetUsersAsync()
            => await context.Users.ToListAsync();

        public async Task<Result<Guid>> AddAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Success(user.Guid);
        }

        public async Task<Result<Guid>> UpdateAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return Success(user.Guid);
        }
    }
}

