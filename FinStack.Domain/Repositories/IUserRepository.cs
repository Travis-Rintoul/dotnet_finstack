using LanguageExt;
using LanguageExt.Common;

using FinStack.Domain.Entities;

namespace FinStack.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Option<User>> GetByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<Result<Guid>> AddAsync(User user);
        Task<Result<Guid>> UpdateAsync(User user);
    }
}