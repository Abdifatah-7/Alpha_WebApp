using System.ComponentModel.DataAnnotations;

namespace Alpha_Webapp.Models;

public class SignInViewModel
{
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter Your email")]
    [Required(ErrorMessage = "You must enter your email")]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "You must enter an valid email adress")]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Your password")]
    [Required(ErrorMessage = "You must enter your password")]
    [RegularExpression(@"^(?=.*[a-ö])(?=.*[A-Ö])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = "You must enter a valid password")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}
