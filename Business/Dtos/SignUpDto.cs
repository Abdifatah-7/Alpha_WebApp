using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class SignUpDto
{


    public string FullName { get; set; } = null!;


    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

  
}


