namespace COMP2139_Friday.Areas.ProjectManagement.Models
{
    public class ProjectTask
    {
        public int ProjectTaskId { get; set; }
        public required string Title { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }  
        public DateTime EndDate { get; set; }    
        public required string Status { get; set; } = "New";

        public int ProjectId { get; set; }
        public Project? Project { get; set; }  // Allow nullable Project
    }
}