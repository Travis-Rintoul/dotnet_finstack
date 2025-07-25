namespace FinStack.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; }
        public string Email { get; }

        public UserDto(Guid id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}