using System.ComponentModel.DataAnnotations;

namespace COMP2139_Friday.Areas.ProjectManagement.Models;

public class Project
{
    // <summary>
    // This is the priamry key for the project
    // </summary>
    public int ProjectId { get; set; }
    
    // <summary>
    // The Name of the project
    // [Required]: Ensures this property must be set
    // </summary>
    [Required]
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    
    public string? Status { get; set; }
    
    public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
}