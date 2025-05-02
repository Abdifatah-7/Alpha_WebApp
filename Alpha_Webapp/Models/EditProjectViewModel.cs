using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Webapp.Models
{
    public class EditProjectViewModel
    {
        public IEnumerable<SelectListItem> Statuses { get; set; } = [];

        public string ProjectId { get; set; } = null!;


        [Required(ErrorMessage = "Project name is required")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; } = null!;

        [Display(Name = "Image URL")]
        [DataType(DataType.Upload)]
        public string? Image { get; set; }

        [Required(ErrorMessage = "You must select a client")]
        [Display(Name = "Client")]
        public string ClientName { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Budget")]
        [DataType(DataType.Currency)]
        public decimal? Budget { get; set; }

        [Required(ErrorMessage = "You must select a status")]
        [Display(Name = "Status")]
        public int Status { get; set; }
    }
}
