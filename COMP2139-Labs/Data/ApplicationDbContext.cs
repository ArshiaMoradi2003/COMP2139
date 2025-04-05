using COMP2139_Labs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using COMP2139_Labs.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace COMP2139_Labs.Data;

public class ApplicationDbContext:IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }
    public DbSet<ProjectComment> ProjectComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Ensure Identity Configurations and Tables are created
        base.OnModelCreating(modelBuilder);
        //Define one-to-many relationship
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks) //One project has (potentially) many tasks.
            .WithOne(t => t.Project) //each projectTask belong to one project
            .HasForeignKey(t => t.ProjectId)//foreign key in projectTask table
            .OnDelete(DeleteBehavior.Cascade);//cascade delete projectTasks when the project is deleted
        
        //Seeding the database
        modelBuilder.Entity<Project>().HasData(
            new Project{ ProjectId = 1, Name = "Assignment 1", Description = "COMP2139 - Assignment 1" },
            new Project{ ProjectId = 2, Name = "Assignment 2", Description = "COMP2139 - Assignment 2" }
            );


    }
}