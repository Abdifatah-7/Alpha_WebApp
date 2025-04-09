using Alpha_Webapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Webapp.Controllers;

public class AuthController : Controller
{
    public IActionResult SignIn()
    {
        var formData = new SignInFormModel();

        return View(formData);
    }

    //Här är hanteras submittet formen 
    [HttpPost]
    public IActionResult SignIn(SignInFormModel formData)
    {
        if (!ModelState.IsValid)
            return View(formData);

        return View();
    }



    public IActionResult SignUp()
    {
        var formData = new SignUpFormModel();
        return View();
    }

    [HttpPost]
    public IActionResult SignUp(SignUpFormModel formData)
    {
        if (!ModelState.IsValid)
            return View(formData);

        return View();
    }



    public new IActionResult SignOut()
    {
        return View();
    }
}
