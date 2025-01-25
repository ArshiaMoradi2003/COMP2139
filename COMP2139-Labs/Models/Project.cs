using System.ComponentModel.DataAnnotations;

namespace COMP2139_Labs.Models;

public class Project
{
    /// <summary>
    /// This is the primary key for projects
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// The Name of the project
    /// [Required]: Ensures this property must be set must have a project name
    /// </summary>
    [Required]
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    
    
    public string? Status { get; set; }
}