using Alpha_Webapp.Models;
using Business.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using System.Security.Claims;

namespace Alpha_Webapp.Controllers;

public class ProjectsController(IProjectService projectService, IStatusService statusService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IStatusService _statusService = statusService;


    [HttpGet]
    public async Task <IActionResult> Index()
    {
        // 1) Initiera model med tom lista
        var model = new ProjectsViewModel
        {
            Projects = new List<ProjectViewModel>(),
            AddProjectFormData = new AddProjectViewModel(),
            EditProjectFormData = new EditProjectViewModel()
        };

        // 2) Hämta från service
        var projectsResult = await _projectService.GetProjectAsync();
        if (projectsResult.Succeeded && projectsResult.Result != null)
        {
            // 3) Mappa och materialisera till List<ProjectViewModel>
            model.Projects = projectsResult
                .Result
                .Select(MapToProjectViewModel)
                .ToList();
        }

        // 4) Fyll status-dropdown (annars blir den tom)
        var statuses = await _statusService.GetStatusesAsync();
        if (statuses.Succeeded && statuses.Result != null)
        {
            model.AddProjectFormData.status = statuses.Result
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.StatusName
                })
                .ToList();
        }

        var all = model.Projects.Count();
        var started = model.Projects.Count(p => p.StatusName == "Started");
        var completed = model.Projects.Count(p => p.StatusName == "Completed");
        ViewBag.TabCounts = (all, started, completed);




        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddProjectViewModel formData)
    {

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Formuläret är inte korrekt ifyllt.";
            
        }


        var AddProjectFormData = new AddProjectViewModel();

 

        // Map the view model to DTO
        var projectDto = new ProjectDto
        {
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            ClientName = formData.ClientName,
            // Convert DateTime to DateOnly for StartDate and EndDate
            StartDate = DateOnly.FromDateTime(formData.StartDate),
            EndDate = DateOnly.FromDateTime(formData.EndDate),
            Budget = formData.Budget,
            Image = formData.Image,
            Status = new StatusDto { Id = formData.Status }
        };

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
        {
            // Inloggad användare
            projectDto.AppUser = new AppUserDto { Id = userId };
        }
        else
        {
            // Ingen inloggad användare - använd en standardvärde
            projectDto.AppUser = new AppUserDto { Id = "default-user" };
        }

        // Call the service to create the project
        var result = await _projectService.CreateProjectAsync(projectDto);


        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = result.Error ?? "An error occurred while creating the project.";
           
        }

        TempData["SuccessMessage"] = "Project created successfully.";
        return RedirectToAction("Index");
    }

    private ProjectViewModel MapToProjectViewModel(ProjectDto dto)
    {
        return new ProjectViewModel
        {
            ProjectId = dto.ProjectId,
            ProjectName = dto.ProjectName,
            Description = dto.Description,
            ClientName = dto.ClientName,
            StatusName = dto.Status?.StatusName ?? "Started",
            Image = dto.Image,
            // Konvertera DateOnly till DateTime
            StartDate = dto.StartDate.ToDateTime(TimeOnly.MinValue),
            EndDate = dto.EndDate.ToDateTime(TimeOnly.MinValue),
            Budget = dto.Budget
        };
    }

}
