namespace Business.Dtos;

public class ClientResult : ServiceResult
{
    public IEnumerable<ClientDto>? Result { get; set; }
}

