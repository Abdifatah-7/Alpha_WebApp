using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class ClientFactory
{
    
    public static ClientEntity? Create(ClientDto form) => form == null ? null : new()
    {
        ClientName = form.ClientName
    };

    public static ClientDto? Create(ClientEntity entity) => entity == null ? null : new()
    {
        ClientId = entity.ClientId,
        ClientName = entity.ClientName
    };

    // Convert Entity collection to DTO collection
    public static IEnumerable<ClientDto> CreateList(IEnumerable<ClientEntity> entities) =>
        entities?.Select(e => Create(e)).Where(dto => dto != null).Select(dto => dto!) ?? new List<ClientDto>();
}
