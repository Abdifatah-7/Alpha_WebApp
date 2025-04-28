using Business.Dtos;
using Business.Factories;
using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(ProjectDto projectDto);
    Task<ProjectResult> DeleteProjectAsync(string id);
    Task<ProjectResult<IEnumerable<ProjectDto>>> GetProjectAsync();
    Task<ProjectResult<ProjectDto>> GetProjectByIdAsync(string id);
    Task<ProjectResult> UpdateProjectAsync(ProjectDto projectDto);
}

public class ProjectService(IProjectRepository projectRepository, IClientRepository clientRepository, IStatusRepository statusRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IStatusRepository _statusRepository = statusRepository;
    public async Task<ProjectResult> CreateProjectAsync(ProjectDto projectDto)
    {
        if (string.IsNullOrWhiteSpace(projectDto.ProjectName))
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Project name is required"
            };

        if (projectDto.EndDate < projectDto.StartDate)
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "End date cannot be before start date"
            };




        var projectExists = await _projectRepository.ExistsAsync(p =>
            p.ProjectName == projectDto.ProjectName &&
            p.ClientId == projectDto.Client.ClientId);

        if (projectExists.Succeeded && projectExists.Result)
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 409, // Conflict är en lämplig statuskod här
                Error = "A project with the same name already exists for the selected client"
            };


        var statusExists = await _statusRepository.ExistsAsync(s => s.Id == projectDto.Status.Id);
        if (!statusExists.Succeeded || !statusExists.Result)
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Vald status finns inte"
            };

        // Skapa projektet
        var entity = new ProjectEntity
        {
            ProjectName = projectDto.ProjectName,
            Description = projectDto.Description,
            StartDate = projectDto.StartDate,
            EndDate = projectDto.EndDate,
            Budget = projectDto.Budget,
            Image = projectDto.Image,
            UserId = projectDto.AppUser.Id,
            ClientId = projectDto.Client.ClientId,
            StatusId = projectDto.Status.Id,
            Created = DateTime.Now
        };

        var result = await _projectRepository.CreateAsync(entity);

        return new ProjectResult
        {
            Succeeded = result.Succeeded,
            StatusCode = result.Succeeded ? 201 : result.StatusCode,
            Error = result.Error
        };
    }

    public async Task<ProjectResult<IEnumerable<ProjectDto>>> GetProjectAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (
                orderByDescending: true,
                sortBy: s => s.Created,
                where: null,
                include => include.User,
                include => include.Client,
                include => include.Status
            );


        if (!response.Succeeded || response.Result == null)
            return new ProjectResult<IEnumerable<ProjectDto>>
            {
                Succeeded = false,
                StatusCode = response.StatusCode,
                Error = "No projects were found"
            };


        var projectDtos = ProjectFactory.CreateList(response.Result);

        return new ProjectResult<IEnumerable<ProjectDto>>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = projectDtos
        };
    }


    public async Task<ProjectResult<ProjectDto>> GetProjectByIdAsync(string id)
    {
        var response = await _projectRepository.GetAsync
            (
                where: x => x.ProjectId == id,
                include => include.User,
                include => include.Client,
                include => include.Status
            );

        if (!response.Succeeded || response.Result == null)
            return new ProjectResult<ProjectDto>
            {
                Succeeded = false,
                StatusCode = response.StatusCode,
                Error = $"Project '{id}' not found"
            };


        var projectDto = ProjectFactory.Create(response.Result);

        return new ProjectResult<ProjectDto>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = projectDto
        };
    }
    //Med hjälp av Cluadi Ai.
    public async Task<ProjectResult> UpdateProjectAsync(ProjectDto projectDto)
    {
        // Validera projektdata
        if (string.IsNullOrWhiteSpace(projectDto.ProjectName))
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Project name is required"
            };

        if (projectDto.EndDate < projectDto.StartDate)
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "End date cannot be before start date"
            };

        // Kontrollera att projektet finns
        var existingProject = await _projectRepository.GetAsync(p => p.ProjectId == projectDto.ProjectId);
        if (!existingProject.Succeeded || existingProject.Result == null)
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Project not found"
            };

        // Kontrollera att klienten finns
        var clientExists = await _clientRepository.ExistsAsync(c => c.ClientId == projectDto.Client.ClientId);
        if (!clientExists.Succeeded || !clientExists.Result)
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Selected client does not exist"
            };

        // Kontrollera att statusen finns
        var statusExists = await _statusRepository.ExistsAsync(s => s.Id == projectDto.Status.Id);
        if (!statusExists.Succeeded || !statusExists.Result)
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Selected status does not exist"
            };

        // Uppdatera projektet
        var entity = existingProject.Result;
        entity.ProjectName = projectDto.ProjectName;
        entity.Description = projectDto.Description;
        entity.StartDate = projectDto.StartDate;
        entity.EndDate = projectDto.EndDate;
        entity.Budget = projectDto.Budget;
        entity.Image = projectDto.Image;
        entity.ClientId = projectDto.Client.ClientId;
        entity.StatusId = projectDto.Status.Id;

        var result = await _projectRepository.UpdateAsync(entity);

        return new ProjectResult
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode,
            Error = result.Error
        };
    }


    public async Task<ProjectResult> DeleteProjectAsync(string id)
    {
        // Kontrollera att projektet finns
        var existingProject = await _projectRepository.GetAsync(p => p.ProjectId == id);
        if (!existingProject.Succeeded || existingProject.Result == null)
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Project not found"
            };

        var result = await _projectRepository.DeleteAsync(existingProject.Result);

        return new ProjectResult
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode,
            Error = result.Error
        };
    }

}

