using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinStack.Domain.Entities
{
    public class AppUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
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

        public AuthUser AuthUser { get; set; }

        public AppUser() { }

        public AppUser(string email, string firstName, string middleName, string lastName)
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