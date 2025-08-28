
namespace FinStack.Contracts.Users
{
    public record GetUsersResponseDto
    {
        public Guid UserGuid { get; set; }
        public string Email { get; set; }

        public string FirstName  { get; set; }
        public string MiddleName  { get; set; }
        public string LastName  { get; set; }
    }
}