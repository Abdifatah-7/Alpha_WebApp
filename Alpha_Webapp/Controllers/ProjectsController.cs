using Microsoft.AspNetCore.Mvc;

namespace Alpha_Webapp.Controllers;

public class ProjectsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
