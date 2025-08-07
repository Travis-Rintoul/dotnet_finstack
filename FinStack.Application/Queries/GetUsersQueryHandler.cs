using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FinStack.Domain.Repositories;
using FinStack.Application.DTOs;

namespace FinStack.Application.Queries
{
    public record GetUsersQuery() : IRequest<IEnumerable<UserDto>>;

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _repo;

        public GetUsersQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repo.GetUsersAsync();
            return users.Select(u => new UserDto(u.Guid, u.Email));
        }
    }
}
