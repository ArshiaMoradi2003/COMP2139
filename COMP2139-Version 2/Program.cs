using COMP2139_Friday.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add the context to the service collection with a connection string
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    app.UseHsts();

    // Use custom status code pages: Redirect 404 errors to Home/NotFound.
    app.UseStatusCodePagesWithReExecute("/Home/NotFound");
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// 🛠 Add Area Routing First
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStaticFiles();

app.Run();