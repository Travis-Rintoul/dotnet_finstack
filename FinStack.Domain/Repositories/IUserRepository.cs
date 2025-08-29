
using FinStack.Common;
using FinStack.Domain.Entities;
using MediatR;

namespace FinStack.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Option<AppUser>> GetByIdAsync(Guid userId);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<Result<Guid>> AddAsync(AppUser user);
        Task<Result<Guid>> UpdateAsync(AppUser user);
        Task<Result<Unit>> DeleteAysnc(AppUser user);
    }
}