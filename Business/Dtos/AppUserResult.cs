namespace Business.Dtos;

public class AppUserResult : ServiceResult
{
    public IEnumerable<AppUserDto>? Result { get; set; }
}

