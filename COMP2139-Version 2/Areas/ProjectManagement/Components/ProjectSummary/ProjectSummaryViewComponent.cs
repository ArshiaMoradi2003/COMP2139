using Microsoft.AspNetCore.Mvc;
using COMP2139_Friday.Data;
using COMP2139_Friday.Areas.ProjectManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace COMP2139_Friday.Areas.ProjectManagement.Components.ProjectSummary;

    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ProjectSummaryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var projects = await _context.Projects.ToListAsync();
            return View(projects);
        }
    }