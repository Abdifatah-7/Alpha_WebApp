namespace Business.Dtos;

public class ProjectDto
{
    public string ProjectId { get; set; } = null!;
    public string? Image { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public string ClientName { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal? Budget { get; set; }
 
    public AppUserDto AppUser { get; set; } = null!;
    public StatusDto Status { get; set; } = null!;
}


