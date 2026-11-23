namespace TaskManager.Web.Models.Authentication;

using System.ComponentModel.DataAnnotations;

public sealed class RegisterFormModel
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(
        100,
        MinimumLength = 3,
        ErrorMessage = "El nombre de usuario debe tener entre 3 y 100 caracteres.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Introduce un correo válido.")]
    [StringLength(
        255,
        ErrorMessage = "El correo no puede superar los 255 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(
        128,
        MinimumLength = 12,
        ErrorMessage = "La contraseña debe tener entre 12 y 128 caracteres.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{12,128}$",
        ErrorMessage = "La contraseña debe incluir mayúsculas, minúsculas y números.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma tu contraseña.")]
    [Compare(
        nameof(Password),
        ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
