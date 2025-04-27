using Business.Dtos;
using Business.Factories;
using Data.Repositories;

namespace Business.Services;

public interface IStatusService
{
    Task<StatusResult> GetStatusesAsync();
}

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult> GetStatusesAsync()
    {
        var result = await _statusRepository.GetAllAsync();

        if (!result.Succeeded || result.Result == null)
            return new StatusResult
            {
                Succeeded = false,
                Error = "No Status was found",
                StatusCode = result.StatusCode
            };

        var statusDtos = StatusFactory.CreateList(result.Result);

        return new StatusResult
        {
            Succeeded = true,
            StatusCode = result.StatusCode,
            Result = statusDtos
        };
    }
}

