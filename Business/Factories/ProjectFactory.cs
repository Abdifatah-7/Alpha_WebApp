using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{

    public static ProjectEntity? Create(ProjectDto form) => form == null ? null : new()
    {
        ProjectName = form.ProjectName,
        Description = form.Description,
        ClientName = form.ClientName,
        StartDate = form.StartDate,
        EndDate = form.EndDate,
        Budget = form.Budget,
        Image = form.Image,
        StatusId = form.Status?.Id ?? 0
    };


    public static ProjectDto? Create(ProjectEntity entity) => entity == null ? null : new()
    {
        ProjectId = entity.ProjectId,
        ProjectName = entity.ProjectName,
        Description = entity.Description,
        ClientName = entity.ClientName,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Budget = entity.Budget,
        Image = entity.Image,
        AppUser = entity.User != null ? AppUserFactory.Create(entity.User) : null!,
        Status = entity.Status != null ? StatusFactory.Create(entity.Status) : null!



    };

    // Convert Entity collection to DTO collection
    public static IEnumerable<ProjectDto> CreateList(IEnumerable<ProjectEntity> entities) =>
        entities?.Select(e => Create(e)).Where(dto => dto != null).Select(dto => dto!) ?? new List<ProjectDto>();
}