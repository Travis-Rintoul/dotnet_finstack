using FinStack.Application.DTOs;
using FinStack.Common;
using FinStack.Domain.Entities;
using FinStack.Domain.Repositories;
using MediatR;

using static FinStack.Common.Result;

namespace FinStack.Application.Commands;

public record CreateUserCommand(UserDto userDto) : IRequest<Result<Guid>>;

public class CreateUserCommandHandler(IUserRepository repo) : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Guid = Guid.NewGuid(),
            Email = request.userDto.Email,
            Name = request.userDto.Email,
            CreatedDate = DateTime.UtcNow
        };
        
        var result = await repo.AddAsync(user);
        
        
        return result.Match(
            guid => Success(guid),
            (ex) => Failure<Guid>(ex)
        );
    }
}