namespace Business.Dtos;

public abstract class ServiceResult
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }
    public int StatusCode { get; set; }
}


