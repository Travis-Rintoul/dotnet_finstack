using FinStack.Application.DTOs;
using FinStack.Common;
using FinStack.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using MediatR;

using static FinStack.Common.Result;

namespace FinStack.Infrastructure.Commands;

public record CreateAuthUserCommand(AuthUserDto authUserDto) : IRequest<Result<Guid>>;

public class CreateUserCommandHandler(UserManager<AuthUser> userManager) : IRequestHandler<CreateAuthUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateAuthUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.authUserDto;
        var user = new AuthUser()
        {
            Id = request.authUserDto.Id.ToString(),
            Email = dto.Email,
            UserName = dto.Email,
            UserType = dto.UserType,
        };
        
        var result = await userManager.CreateAsync(user,  dto.Password);
        if (!result.Succeeded)
        {
            return Failure<Guid>(result.Errors.Select(e => new Exception(e.Code)));
        }
        
        return Success(dto.Id);
    }
}