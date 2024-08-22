using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AutoMapper;
using PRN231_API.Repositories; // Adjust namespace as needed
using PRN231_API.DAO; // Adjust namespace as needed
using PRN231_API.Models;
using Microsoft.EntityFrameworkCore;
using PRN231_API.Mapping; // Adjust namespace as needed

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Register DbContext
builder.Services.AddDbContext<SchoolDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DAOs
builder.Services.AddScoped<IStudentDAO, StudentDAO>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();


// Register Repositories
builder.Services.AddScoped<IEvaluationDAO, EvaluationDAO>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();

// Add controllers and configure Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
// Add this to handle HTTP method override
app.UseHttpMethodOverride();
app.MapControllers();

app.Run();