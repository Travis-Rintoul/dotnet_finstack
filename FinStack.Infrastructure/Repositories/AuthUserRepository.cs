using FinStack.Common;
using FinStack.Domain.Entities;
using FinStack.Infrastructure.Data;
using FinStack.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using static FinStack.Common.Result;
using MediatR;

namespace FinStack.Infrastructure.Repositories
{
    public class AuthUserRepository(AppDbContext context) : IAuthUserRepository
    {
        public Task<Result<Guid>> AddAsync(AuthUser user)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Unit>> DeleteAysnc(AuthUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<Option<AuthUser>> GetByIdAsync(Guid userId)
        {
            return new Option<AuthUser>(await context.Users.SingleOrDefaultAsync(u => u.Id == userId));
        }

        public Task<IEnumerable<AuthUser>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Guid>> UpdateAsync(AuthUser user)
        {
            throw new NotImplementedException();
        }
    }
}

