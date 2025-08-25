using FinStack.Domain.Repositories;
using MediatR;
using FinStack.Application.DTOs;
using FinStack.Common;

namespace FinStack.Application.Queries
{
    public record GetAuthUserByIdQuery(Guid UserId) : IRequest<Option<AuthUserDto>>;

    public class GetAuthUserByIdQueryHandler : IRequestHandler<GetAuthUserByIdQuery, Option<AuthUserDto>>
    {
        private readonly IAuthUserRepository _repo;

        public GetAuthUserByIdQueryHandler(IAuthUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<Option<AuthUserDto>> Handle(GetAuthUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userOption = await _repo.GetByIdAsync(request.UserId);
            return userOption.Match(
                user => new AuthUserDto { UserGuid = user.Id },
                () => Option<AuthUserDto>.None()
            );
        }
    }
}
