using FinStack.Common;
using FinStack.Domain.Entities;
using FinStack.Infrastructure.Data;
using FinStack.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using static FinStack.Common.Result;
using MediatR;

namespace FinStack.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<Option<AppUser>> GetByIdAsync(Guid userGuid)
        {
            return new Option<AppUser>(await context.AppUsers.SingleOrDefaultAsync(u => u.UserGuid == userGuid));
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
            => await context.AppUsers.ToListAsync();

        public async Task<Result<Guid>> AddAsync(AppUser user)
        {
            await context.AppUsers.AddAsync(user);
            await context.SaveChangesAsync();
            return Success(user.UserGuid);
        }

        public async Task<Result<Guid>> UpdateAsync(AppUser user)
        {
            context.AppUsers.Update(user);
            await context.SaveChangesAsync();
            return Success(user.UserGuid);
        }

        public async Task<Result<Unit>> DeleteAysnc(AppUser user)
        {
            context.AppUsers.Remove(user);
            await context.SaveChangesAsync();
            return Success(new Unit());
        }
    }
}

