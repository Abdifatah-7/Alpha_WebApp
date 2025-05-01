using Alpha_Webapp.Models;
using Business.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Webapp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;



   
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Konvertera ViewModel till DTO
        var signUpDto = new SignUpDto
        {
            FullName = model.FullName,
            Email = model.Email,
            Password = model.Password
        };

        // Anropa service för att registrera användaren
        var result = await _authService.SignUpAsync(signUpDto);

        if (result.Succeeded)
        {
        // Omdirigera till inloggningssidan efter lyckad registrering
        return RedirectToAction("SignIn","Auth");

        }

        // Om registreringen misslyckades, visa felet
        ViewBag.ErrorMessage = result.Error;
            return View(model);
    }

    
    public IActionResult SignIn()
    {
        return View();
    }

   
    [HttpPost]
    
    public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = null;
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var signInDto = new SignInDto
        {
            Email = model.Email,
            Password = model.Password,
            RememberMe = model.RememberMe
        };

        var result = await _authService.SignInAsync(signInDto);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Projects");

        }

        ViewBag.ErrorMessage = result.Error;
        return View(model);

    }



    public async Task<IActionResult> SignOut()
    {
        // Logga ut användaren
        await _authService.SignOutAsync();

        // Omdirigera till startsidan
        return RedirectToAction("SignIn", "Auth");
    }
}
