using System.ComponentModel.DataAnnotations;
using FinStack.Domain.Entities;

public class UserPreferences
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public RiskLevel RiskLevel { get; set; } = RiskLevel.Medium;
    public string[] PreferredMarkets { get; set; } = Array.Empty<string>();
    public string DefaultCurrency { get; set; } = Currency.AUD;
    public bool EmailNotifications { get; set; } = true;
    public bool PushNotifications { get; set; } = true;
    public Theme Theme { get; set; } = Theme.Light;
}
