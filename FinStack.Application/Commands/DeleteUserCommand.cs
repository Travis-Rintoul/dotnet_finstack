using FinStack.Application.DTOs;
using FinStack.Common;
using FinStack.Domain.Entities;
using FinStack.Domain.Repositories;
using MediatR;

using static FinStack.Common.Result;

namespace FinStack.Application.Commands;

public record DeleteUserCommand(Guid userGuid) : IRequest<Result<Unit>>;

public class DeleteUserCommandHandler(IUserRepository repo) : IRequestHandler<DeleteUserCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        Option<AppUser> option = await repo.GetByIdAsync(request.userGuid);
        if (option.IsNone)
        {
            return Failure<Unit>(Error.UserNotFound(request.userGuid));
        }

        return await repo.DeleteAysnc(option.Value);
    }
}