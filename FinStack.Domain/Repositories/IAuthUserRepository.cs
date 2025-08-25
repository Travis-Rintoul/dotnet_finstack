
using FinStack.Common;
using FinStack.Domain.Entities;
using MediatR;

namespace FinStack.Domain.Repositories
{
    public interface IAuthUserRepository
    {
        Task<Option<AuthUser>> GetByIdAsync(Guid userId);
        Task<IEnumerable<AuthUser>> GetUsersAsync();
        Task<Result<Guid>> AddAsync(AuthUser user);
        Task<Result<Guid>> UpdateAsync(AuthUser user);
        Task<Result<Unit>> DeleteAysnc(AuthUser user);
    }
}