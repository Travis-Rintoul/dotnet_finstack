namespace FinStack.Application.DTOs;
public class UserDto
{
    public UserDto() {}
    public Guid UserGuid { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; }
}
