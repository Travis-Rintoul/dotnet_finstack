using FinStack.Application.DTOs;
using FinStack.Common;
using FinStack.Domain.Entities;
using FinStack.Domain.Repositories;
using MediatR;

using static FinStack.Common.Result;

namespace FinStack.Application.Commands;

public record UpdateUserCommand(Guid userGuid, UpdateUserDto userDto) : IRequest<Result<Guid>>;

public class UpdateUserCommandHandler(IUserRepository repo) : IRequestHandler<UpdateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.userDto;
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(dto.FirstName))
        {
            errors.Add(Error.FirstNameRequired);
        }

        if (string.IsNullOrWhiteSpace(dto.LastName))
        {
            errors.Add(Error.LastNameRequired);
        }

        if (errors.Any())
        {
            return Failure<Guid>(errors);
        }

        Option<AppUser> option = await repo.GetByIdAsync(request.userGuid);
        if (option.IsNone)
        {
            return Failure<Guid>(Error.UserNotFound(request.userGuid));
        }

        AppUser user = option.Unwrap();
        user.FirstName = dto.FirstName;
        user.MiddleName = dto.MiddleName;
        user.LastName = dto.LastName;

        var result = await repo.UpdateAsync(user);
        return result.Match(
            guid => Success(guid),
            (ex) => Failure<Guid>(ex)
        );
    }
}