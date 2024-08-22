using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient(); // Register HttpClient for dependency injection

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=StudentList}/{subjectId?}");

app.MapControllerRoute(
    name: "evaluations",
    pattern: "student/{studentId}/evaluations",
    defaults: new { controller = "Student", action = "StudentEvaluations" });

app.MapControllerRoute(
    name: "editEvaluation",
    pattern: "student/editEvaluation",
    defaults: new { controller = "Student", action = "EditEvaluation" });

app.MapRazorPages();

app.Run();
