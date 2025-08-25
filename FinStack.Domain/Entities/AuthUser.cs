using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinStack.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace FinStack.Domain.Entities;

public class AuthUser : IdentityUser<Guid>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AuthUserId { get; set; }
    public User AppUser { get; set; }

    [Required]
    [MaxLength(30)]
    public override string Email { get; set; } = string.Empty;

    [Required]
    public UserType UserType { get; set; } = UserType.Individual;
}