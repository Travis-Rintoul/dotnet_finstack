using FinStack.Domain.Enums;

namespace FinStack.Application.DTOs;

public class AuthUserDto
{
    public Guid UserGuid { get; set; }= Guid.Empty;
    public string Email { get; set; } = string.Empty;
    public UserType UserType { get; set; } = UserType.Individual;
    public string Password { get; set; } = string.Empty;
    
}