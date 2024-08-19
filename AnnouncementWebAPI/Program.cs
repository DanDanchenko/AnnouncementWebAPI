using Microsoft.EntityFrameworkCore;
using AnnouncementWebAPI.Models;
using AnnouncementWebAPI.Repositories;
using AnnouncementWebAPI.Services;
using AnnouncementWebAPI.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AnnouncementContext>(opt => opt.UseInMemoryDatabase("ListOfAnnouncements"));
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<AnnouncementRepository>();
builder.Services.AddScoped <AnnouncementService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
