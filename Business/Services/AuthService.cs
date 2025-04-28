using Business.Dtos;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IAuthService
{
    Task<AuthResult> SignInAsync(SignInDto signInDto);
    Task<AuthResult> SignOutAsync();
    Task<AuthResult> SignUpAsync(SignUpDto signUpDto);
}

public class AuthService(UserManager<AppUserEntity> userManager, SignInManager<AppUserEntity> signInManager) : IAuthService
{
    private readonly UserManager<AppUserEntity> _userManager = userManager;
    private readonly SignInManager<AppUserEntity> _signInManager = signInManager;

    public async Task<AuthResult> SignUpAsync(SignUpDto signUpDto)
    {
        if (string.IsNullOrWhiteSpace(signUpDto.FullName) ||
          string.IsNullOrWhiteSpace(signUpDto.Email) ||
          string.IsNullOrWhiteSpace(signUpDto.Password))
        {
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "All fields are required"
            };
        }



        // Kontrollera om användaren redan finns
        var existingUser = await _userManager.FindByEmailAsync(signUpDto.Email);
        if (existingUser != null)
        {
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = 409,
                Error = "A user with this email already exists"
            };
        }

        // Skapa ny användare
        var user = new AppUserEntity
        {
            FirstName = signUpDto.FullName,
            UserName = signUpDto.Email,
            Email = signUpDto.Email,
            LastName = " "
        };

        var result = await _userManager.CreateAsync(user, signUpDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = $"Failed to create user: {errors}"
            };
        }

        return new AuthResult
        {
            Succeeded = true,
            StatusCode = 201  // Created
        };
    }


    /// SignIn


    public async Task<AuthResult> SignInAsync(SignInDto signInDto)
    {
        // Validera indata
        if (string.IsNullOrWhiteSpace(signInDto.Email) ||
            string.IsNullOrWhiteSpace(signInDto.Password))
        {
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Email and password are required"
            };
        }

        // Försök hitta användaren med e-post
        var user = await _userManager.FindByEmailAsync(signInDto.Email);
        if (user == null)
        {
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = 401,
                Error = "Invalid email or password"
            };
        }

        // Försök logga in användaren
        var result = await _signInManager.PasswordSignInAsync(
            user,
            signInDto.Password,
            isPersistent: signInDto.RememberMe,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = 401,
                Error = "Invalid email or password"
            };
        }

        // Användaren har loggats in framgångsrikt
        return new AuthResult
        {
            Succeeded = true,
            StatusCode = 200
        };
    }

    //SignOut


    public async Task<AuthResult> SignOutAsync()
    {
        await _signInManager.SignOutAsync();

        return new AuthResult
        {
            Succeeded = true,
            StatusCode = 200
        };
    }

}

