using FinStack.Domain.Enums;

namespace FinStack.Application.DTOs;

public class AuthUserDto
{
    public Guid Id { get; set; }= Guid.Empty;
    public string Email { get; set; } = string.Empty;
    public UserType Type { get; set; } = UserType.User;
    public string Password { get; set; } = string.Empty;
    
}