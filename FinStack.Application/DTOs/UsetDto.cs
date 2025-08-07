namespace FinStack.Application.DTOs;
public class UserDto
{
    public UserDto() {}
    public UserDto(Guid guid, string email)
    {
        Id = guid;
        Email = email;
    }
    
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
