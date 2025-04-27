using Business.Dtos;
using Business.Factories;
using Data.Repositories;

namespace Business.Services;

public interface IAppUserService
{
    Task<AppUserResult> GetAppUsersAsync();
    Task<AppUserResult> GetByEmailAsync(string email);
    Task<AppUserResult> GetByIdAsync(string id);
}

public class AppUserService(AppUserRepository appUserRepository) : IAppUserService
{
    private readonly AppUserRepository _appUserRepository = appUserRepository;

    public async Task<AppUserResult> GetAppUsersAsync()
    {
        var result = await _appUserRepository.GetAllAsync();
        if (!result.Succeeded || result.Result == null)
            return new AppUserResult
            {
                Succeeded = false,
                Error = "No Clients were found",
                StatusCode = result.StatusCode
            };
        var dto = AppUserFactory.CreateList(result.Result);
        return new AppUserResult
        {
            Succeeded = true,
            StatusCode = result.StatusCode,
            Result = dto
        };
    }

    public async Task<AppUserResult> GetByIdAsync(string id)
    {
        var result = await _appUserRepository.GetAsync(u => u.Id == id);

        if (!result.Succeeded || result.Result == null)
            return new AppUserResult
            {
                Succeeded = false,
                StatusCode = result.StatusCode,
                Error = "User not found"
            };

        var dto = AppUserFactory.Create(result.Result);
        return new AppUserResult
        {
            Succeeded = true,
            StatusCode = 200,
            Result = new List<AppUserDto> { dto! }
        };
    }

    public async Task<AppUserResult> GetByEmailAsync(string email)
    {
        var result = await _appUserRepository.GetAsync(u => u.Email == email);

        if (!result.Succeeded)
            return new AppUserResult
            {
                Succeeded = false,
                StatusCode = result.StatusCode,
                Error = "User not found"
            };

        var dto = AppUserFactory.Create(result.Result!);
        return new AppUserResult
        {
            Succeeded = true,
            StatusCode = 200,
            Result = new List<AppUserDto> { dto! }
        };
    }
}

