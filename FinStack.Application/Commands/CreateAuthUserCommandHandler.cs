using FinStack.Application.DTOs;
using FinStack.Common;
using MediatR;

using static FinStack.Common.Result;
using Microsoft.EntityFrameworkCore;
using FinStack.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using FinStack.Application.Queries;

namespace FinStack.Application.Commands;

public record CreateAuthUserCommand(AuthUserDto authUserDto) : IRequest<Result<Guid>>;

public class CreateAuthUserCommandHandler(UserManager<AuthUser> userManager, IMediator mediator) : IRequestHandler<CreateAuthUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateAuthUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.authUserDto;
        var exists = (await mediator.Send(new GetAuthUserByIdQuery(dto.UserGuid))).IsSome;
        if (exists)
        {
            return Failure<Guid>(Error.UserAlreadyExists(dto.Email));
        }

        var user = new AuthUser()
        {
            Email = dto.Email,
            UserName = dto.Email,
            UserType = dto.UserType,
        };
        
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return Failure<Guid>(result.Errors);
        }

        return Success(user.Id);
    }
}