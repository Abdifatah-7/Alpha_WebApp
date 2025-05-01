using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public string ProjectId { get; set; } = Guid .NewGuid().ToString();

    public string? Image { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public string ClientName { get; set; } = null!;

    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }

    public decimal? Budget { get; set; }

    public DateTime Created {  get; set; } =DateTime.Now;


    [ForeignKey(nameof(User))]
    public string UserId { get; set; } =null!;
    public AppUserEntity User { get; set; } = null!;

    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;

}
