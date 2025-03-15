﻿using COMP2139_Friday.Models;
using COMP2139_Friday.Areas.ProjectManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Friday.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks {get; set;}
    // Add DbSet for other entities like Tasks in the future.
}