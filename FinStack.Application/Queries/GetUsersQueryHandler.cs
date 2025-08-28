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
        private readonly IUserRepository _usersRepo;
        private readonly IAuthUserRepository _authUsersRepo;

        public GetUsersQueryHandler(IUserRepository usersRepo, IAuthUserRepository authUsersRepo)
        {
            _usersRepo = usersRepo;
            _authUsersRepo = authUsersRepo;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _usersRepo.GetUsersAsync();
            var authUsers = await _authUsersRepo.GetUsersAsync();

            return users.Select(user =>
            {
                var authUser = authUsers.SingleOrDefault(auth => user.UserGuid == auth.Id);

                return new UserDto
                {
                    UserGuid = user.UserGuid,
                    Email = authUser!.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
            });
        }
    }
}
