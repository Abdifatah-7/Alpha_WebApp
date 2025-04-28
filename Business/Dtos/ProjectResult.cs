namespace Business.Dtos;

public class ProjectResult<T>: ServiceResult
{
    public T? Result { get; set; }
}

public class ProjectResult : ServiceResult
{
}