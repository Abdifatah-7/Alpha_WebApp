namespace Data.Model;

public class RepositoryResult<T>
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }
    public int StatusCode { get; set; }
    public T? Result { get; set; }
}
