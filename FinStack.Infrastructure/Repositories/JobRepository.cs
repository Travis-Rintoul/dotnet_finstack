using FinStack.Common;
using FinStack.Domain.Entities;
using FinStack.Infrastructure.Data;
using FinStack.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using static FinStack.Common.Result;
using MediatR;

namespace FinStack.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public JobRepository(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Task<Result<Job>> AddAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<Option<Job>> GetByIdAsync(Guid jobId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Job>> GetJobsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Option<Job>> PollAsync(Guid jobGuid, 
            TimeSpan? timeout = null, TimeSpan? pollInterval = null, 
            CancellationToken ct = default)
        {
            var until = DateTime.UtcNow + (timeout ?? TimeSpan.FromSeconds(10));
            var delay = pollInterval ?? TimeSpan.FromMilliseconds(250);

            while (DateTime.UtcNow < until)
            {
                ct.ThrowIfCancellationRequested();

                await using var db = await _dbFactory.CreateDbContextAsync(ct);

                var job = await db.Set<Job>()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(j => j.Guid == jobGuid, ct);

                if (job is not null)
                    return new Option<Job>(job);

                await Task.Delay(delay, ct);
            }

            return new Option<Job>(null); // or Option.None depending on your type
        }

        public Task<Result<Job>> UpdateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}

