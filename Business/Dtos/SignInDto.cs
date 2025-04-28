using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;




public class SignInDto
{

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}


