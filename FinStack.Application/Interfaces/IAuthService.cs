
using FinStack.Application.DTOs;
using FinStack.Common;
using FinStack.Contracts.Auth;

namespace FinStack.Application.Interfaces;

public interface IAuthService
{
    public Task<Result<LoginUserResponseDto>> LoginAsync(string email, string password);
    public Task<Result<Guid>> RegisterAsync(RegisterUserDto dto);
    public Task<Result<Guid>> UpdateAsync(AuthUserDto userDto);
}