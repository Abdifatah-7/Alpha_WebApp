using System.ComponentModel.DataAnnotations;

namespace Alpha_Webapp.Models;

public class SignUpViewModel
{

    [Display(Name = "FullName", Prompt = "Enter Your fullname")]
    [Required(ErrorMessage = "You must enter your fullname")]
    public string FullName { get; set; } = null!;


    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter Your email")]
    [Required(ErrorMessage = "You must enter your email")]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "You must enter an valid email adress")]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Your password")]
    [RegularExpression(@"^(?=.*[a-ö])(?=.*[A-Ö])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = "You must enter strong password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "You must confirm your password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmpassword", Prompt = "Confirm Your password")]
    [Compare(nameof(Password),ErrorMessage ="Your password do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Display(Name ="Terms and Conditions ",Prompt= "I accept the terms and conditions. ")]
    [Range(typeof(bool) ,"true","true", ErrorMessage ="You must accept the terms and conditions in order to go an")]
    [Required(ErrorMessage = "You must accept the terms and conditions.")]
    public bool TermsAndConditions { get; set; }
}
