using System.ComponentModel.DataAnnotations;
using FinStack.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace FinStack.Infrastructure.Entities;

public class AuthUser : IdentityUser
{
    [Required] 
    [MaxLength(30)] 
    public override string Email { get; set; } = string.Empty;
    
    [Required] 
    public UserType Type { get; set; } = UserType.User;
}