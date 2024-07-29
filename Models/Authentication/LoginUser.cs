using System.ComponentModel.DataAnnotations;


namespace eBook_Library.Models.Authentication;

public class LoginUser {

    [EmailAddress]
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}