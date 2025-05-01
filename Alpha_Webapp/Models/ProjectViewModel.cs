namespace Alpha_Webapp.Models;

public class ProjectViewModel
{
    public string? ProjectId { get; set; } = Guid.NewGuid().ToString();
    public string? ProjectName { get; set; }
    public string? Image { get; set; }
    public string? ClientName { get; set; }
    public string? Description { get; set; }


    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? Budget { get; set; }
    public string StatusName { get; set; } = "Started";

}
