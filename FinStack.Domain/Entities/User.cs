using System.ComponentModel.DataAnnotations;

namespace FinStack.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public Guid UserGuid { get; set; }
        
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Nationality { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; }
        
        public User() { }

        public User(string email, string firstName, string middleName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required");
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("FirstName is required");
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("LastName is required");

            UserGuid = Guid.NewGuid();
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            CreatedDate = DateTime.UtcNow;
        }
    }
}