namespace FinStack.Application.DTOs;
public class UserDto(Guid id, string email)
{
    public Guid Id { get; } = id;
    public string Email { get; } = email;
}
