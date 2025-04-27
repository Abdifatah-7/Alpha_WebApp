using Business.Dtos;
using Business.Factories;
using Data.Repositories;

namespace Business.Services;

public interface IClientService
{
    Task<ClientResult> GetByIdAsync(int id);
    Task<ClientResult> GetClientsAsync();
}

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<ClientResult> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        if (!result.Succeeded || result.Result == null)
            return new ClientResult
            {
                Succeeded = false,
                Error = "No Clients were found",
                StatusCode = result.StatusCode
            };
        var clientDtos = ClientFactory.CreateList(result.Result);
        return new ClientResult
        {
            Succeeded = true,
            StatusCode = result.StatusCode,
            Result = clientDtos
        };
    }


    public async Task<ClientResult> GetByIdAsync(int id)
    {
        var result = await _clientRepository.GetAsync(c => c.ClientId == id);

        if (!result.Succeeded || result.Result == null)
            return new ClientResult
            {
                Succeeded = false,
                StatusCode = result.StatusCode,
                Error = "Client not found"
            };

        var clientDtos = new List<ClientDto> { ClientFactory.Create(result.Result)! };
        return new ClientResult
        {
            Succeeded = true,
            StatusCode = result.StatusCode,
            Result = clientDtos
        };
    }

}

