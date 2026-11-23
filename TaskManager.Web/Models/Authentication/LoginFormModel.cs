namespace TaskManager.Web.Models.Authentication;

using System.ComponentModel.DataAnnotations;

public sealed class LoginFormModel
{
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [StringLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(128)]
    public string Password { get; set; } = string.Empty;
}
