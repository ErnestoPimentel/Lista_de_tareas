using System.ComponentModel.DataAnnotations;

namespace TaskManager.Identity.Models.Authentication;

public sealed record RegisterRequest(
    [Required]
    [StringLength(100, MinimumLength = 3)]
    string UserName,

    [Required]
    [EmailAddress]
    [StringLength(255)]
    string Email,

    [Required]
    [StringLength(128, MinimumLength = 12)]
    string Password);
