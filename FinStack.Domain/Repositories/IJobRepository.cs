
using FinStack.Common;
using FinStack.Domain.Entities;

namespace FinStack.Domain.Repositories
{
    public interface IJobRepository
    {
        Task<Option<Job>> GetByIdAsync(Guid jobId);
        Task<IEnumerable<Job>> GetJobsAsync();
        Task<Result<Job>> AddAsync(User user);
        Task<Result<Job>> UpdateAsync(User user);
    }
}