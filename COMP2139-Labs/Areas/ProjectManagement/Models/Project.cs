using System.ComponentModel.DataAnnotations;

namespace COMP2139_Labs.Areas.ProjectManagement.Models;

public class Project
{
    /// <summary>
    /// The unique primary key for projects
    /// </summary>
    public int ProjectId { get; set; }
    /// <summary>
    /// The name of the project
    /// Requires - Ensures the property must be provided (must have a value)
    /// </summary>
    [Display(Name = "Project Name")]
    [Required]
    [StringLength(100, ErrorMessage = "Project Name cannot be longer than 100 characters.")]
    public required string Name { get; set; }
   
    
    
    [Display(Name = "Project Description")]
    [StringLength(500, ErrorMessage = "Project Description cannot be longer than 100 characters.")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }//can be null
    
    [Display(Name = "Project Start Date")]
    [DataType(DataType.Date)]//default formatting
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime StartDate { get; set; }
    
    [Display(Name = "Project End Date")]
    [DataType(DataType.Date)]//default formatting
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime EndDate { get; set; }
   
    [Display(Name = "Project Status")]
    public string? Status { get; set; }
    
    //one-to-many: a project can have many tasks
    public List<ProjectTask>? Tasks { get; set; } = new();

}