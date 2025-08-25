
using FinStack.Common;
using FinStack.Domain.Entities;
using MediatR;

namespace FinStack.Domain.Repositories
{
    public interface IUserPreferenceRepository
    {
        Task<Option<UserPreference>> GetByUserIdAsync(Guid userId);
        Task<Result<Unit>> AddOrUpdateAsync(UserPreference preference);
        Task<Result<Unit>> DeleteAysnc(UserPreference preference);
    }
}