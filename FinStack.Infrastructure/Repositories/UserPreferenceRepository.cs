using FinStack.Common;
using FinStack.Domain.Entities;
using FinStack.Infrastructure.Data;
using FinStack.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using static FinStack.Common.Result;
using MediatR;

namespace FinStack.Infrastructure.Repositories
{
    public class UserPreferenceRepository(AppDbContext context) : IUserPreferenceRepository
    {
        public async Task<Result<Unit>> AddOrUpdateAsync(UserPreference preference)
        {
            var existing = await context.UserPreferences.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == preference.UserId);
            if (existing == null)
            {
                await context.UserPreferences.AddAsync(preference);
            }
            else
            {
                context.UserPreferences.Update(preference);
                await context.SaveChangesAsync();
            }

            return Success(new Unit());
        }

        public Task<Result<Unit>> DeleteAysnc(UserPreference preference)
        {
            throw new NotImplementedException();
        }

        public Task<Option<UserPreference>> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}

