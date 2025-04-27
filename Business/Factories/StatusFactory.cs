using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class StatusFactory
{
    public static StatusEntity? Create(StatusDto form) => form == null ? null : new()
    {
        StatusName = form.StatusName
    };

    public static StatusDto? Create(StatusEntity entity) => entity == null ? null : new()
    {
        Id = entity.Id,
        StatusName = entity.StatusName
    };

    public static IEnumerable<StatusDto> CreateList(IEnumerable<StatusEntity> entities) => entities?.Select(e => Create(e)).Where(dto => dto != null).Select(dto => dto!) ?? new List<StatusDto>();
}
