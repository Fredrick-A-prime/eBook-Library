using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;



namespace eBook_Library.Models.Authentication;
public class UserModel {

    [Required]
    public string UserName { get; set;} = string.Empty;

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
    
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}