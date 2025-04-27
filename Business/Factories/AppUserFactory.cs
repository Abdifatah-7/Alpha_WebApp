using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class AppUserFactory
{
    public static AppUserDto? Create(AppUserEntity entity) => entity == null ? null : new()
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Email = entity.Email!
    };


    // Convert Entity collection to DTO collection
    public static IEnumerable<AppUserDto> CreateList(IEnumerable<AppUserEntity> entities) =>
        entities?.Select(e => Create(e)).Where(dto => dto != null).Select(dto => dto!) ?? new List<AppUserDto>();
}
