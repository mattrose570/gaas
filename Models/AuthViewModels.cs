using System.ComponentModel.DataAnnotations;

namespace gaas.Models;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";
}

public class SignUpViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, StringLength(128, MinimumLength = 12)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required, DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = "";
}