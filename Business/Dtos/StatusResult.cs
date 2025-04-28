namespace Business.Dtos;

public class StatusResult : ServiceResult
{
    public IEnumerable<StatusDto>? Result { get; set; }
}
