
using FinStack.Common;
using FinStack.Domain.Entities;
using MediatR;

namespace FinStack.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Option<User>> GetByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<Result<Guid>> AddAsync(User user);
        Task<Result<Guid>> UpdateAsync(User user);
        Task<Result<Unit>> DeleteAysnc(User user);
    }
}