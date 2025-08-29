

namespace FinStack.Contracts.Auth
{
    public record RegisterUseRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}