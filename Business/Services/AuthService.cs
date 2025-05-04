using Business.Dtos;
using Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Business.Services;

public interface IAuthService
{
    Task<AuthResult> SignInAsync(SignInDto signInDto);
    Task<AuthResult> SignOutAsync();
    Task<AuthResult> SignUpAsync(SignUpDto signUpDto);
}

public class AuthService(UserManager<AppUserEntity> userManager, SignInManager<AppUserEntity> signInManager, IHttpContextAccessor httpContextAccessor =null!) : IAuthService
{
    private readonly UserManager<AppUserEntity> _userManager = userManager;
    private readonly SignInManager<AppUserEntity> _signInManager = signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

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
        if (string.IsNullOrWhiteSpace(signInDto.Email) || string.IsNullOrWhiteSpace(signInDto.Password))
        {
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Email and password are required"
            };
        }

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

        var result = await _signInManager.PasswordSignInAsync(user, signInDto.Password, signInDto.RememberMe, false);
        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = 401,
                Error = "Invalid email or password"
            };
        }

        //Med hjälp av ChatGPT
        // Lägg till FullName som en claim
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.FirstName), // eller använd en sammanslagen FullName
        new Claim(ClaimTypes.Email, user.Email!)
    };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

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

