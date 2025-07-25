using System;
using System.Threading;
using System.Threading.Tasks;
using FinStack.Domain.Repositories;
using MediatR;
using FinStack.Application.Dtos;
using LanguageExt;

namespace FinStack.Application.Queries
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<Option<UserDto>>;

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Option<UserDto>>
    {
        private readonly IUserRepository _repo;

        public GetUserByIdQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<Option<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userOption = await _repo.GetByIdAsync(request.UserId);

            return userOption.Match(
                user => new UserDto(user.Guid, user.Email),
                () => Option<UserDto>.None
            );
        }
    }
}
