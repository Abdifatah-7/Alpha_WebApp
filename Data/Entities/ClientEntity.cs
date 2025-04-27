using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(ClientName), IsUnique=true)]
public class ClientEntity
{
    [Key]
    public int ClientId { get; set; }
    public string ClientName { get; set; } = null!;

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
