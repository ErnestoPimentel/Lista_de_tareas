using System.ComponentModel.DataAnnotations;

namespace TaskManager.Contracts.Authentication;

public sealed record LoginRequest(
    [Required]
    [StringLength(100)]
    string UserName,

    [Required]
    [StringLength(128, MinimumLength = 1)]
    string Password);

