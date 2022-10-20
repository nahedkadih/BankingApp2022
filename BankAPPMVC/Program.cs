using BankApp.Data;
using BankApp.Models;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

 
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BankContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

 

 


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
