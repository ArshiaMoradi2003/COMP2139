using System.ComponentModel.DataAnnotations;

namespace COMP2139_Labs.Areas.ProjectManagement.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Display(Name = "Task Title")]
    [Required]
    [StringLength(100, ErrorMessage = "Task Title cannot be longer than 100 characters.")]
    public required string Title { get; set; }
    
    [Display(Name = "Project Task Description")]
    [StringLength(500, ErrorMessage = "Task Description cannot be longer than 100 characters.")]
    [DataType(DataType.MultilineText)]
    [Required]
    public required string Description { get; set; }
    
    //foreign key
    [Display(Name = "Parent Project Id")]
    public int ProjectId { get; set; }
    
    //navigation property
    //this property allows for easy access to the related Project entity from Task entity
    public Project?  Project { get; set; }
}