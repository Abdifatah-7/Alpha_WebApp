using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{

    public static ProjectEntity? Create(ProjectDto form) => form == null ? null : new()
    {
        ProjectName = form.ProjectName,
        Description = form.Description,
        StartDate = form.StartDate,
        EndDate = form.EndDate,
        Budget = form.Budget,
        Image = form.Image,
        ClientId = form.Client?.ClientId ?? 0,
        StatusId = form.Status?.Id ?? 0
    };


    public static ProjectDto? Create(ProjectEntity entity) => entity == null ? null : new()
    {
        ProjectId = entity.ProjectId,
        ProjectName = entity.ProjectName,
        Description = entity.Description,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Budget = entity.Budget,
        Image = entity.Image,
        Client = entity.Client != null ? ClientFactory.Create(entity.Client) : null!,
        AppUser = entity.User != null ? AppUserFactory.Create(entity.User) : null!,
        Status = entity.Status != null ? StatusFactory.Create(entity.Status) : null!



    };

    // Convert Entity collection to DTO collection
    public static IEnumerable<ProjectDto> CreateList(IEnumerable<ProjectEntity> entities) =>
        entities?.Select(e => Create(e)).Where(dto => dto != null).Select(dto => dto!) ?? new List<ProjectDto>();
}