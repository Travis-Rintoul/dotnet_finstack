
using FinStack.Application.DTOs;
using FinStack.Common;

namespace FinStack.Application.Interfaces;

public interface IAuthService
{
    public Task<Option<AuthResponseDto>> LoginAsync(string email, string password);
    public Task<Result<Guid>> RegisterAsync(RegisterUserDto dto);
    public Task<Result<Guid>> UpdateAsync(AuthUserDto userDto);
}