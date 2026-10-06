using System.ComponentModel.DataAnnotations;

namespace Desafio3.DTOs;

public class AdminSeedOptions
{
    [Required]
    [EmailAddress]
    public string AdminEmail { get; set; } = string.Empty;

    public string? AdminPassword { get; set; }
}
