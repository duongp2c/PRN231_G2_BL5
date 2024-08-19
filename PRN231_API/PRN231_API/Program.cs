using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PRN231_API.DAO;
using PRN231_API.Models;
using PRN231_API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("CORSPolicy", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed((host) => true));
});

builder.Services.AddControllers().AddOData(option => option.Select().Filter().Count().OrderBy().Expand().SetMaxTop(100)
          .AddRouteComponents("odata", GetEdmModel()));

builder.Services.AddDbContext<SchoolDBContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<SchoolDBContext>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<AccountDAO>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<StudentDAO>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<SubjectDAO>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors("CORSPolicy");

app.UseODataBatching();

app.MapControllers();

app.Run();
IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Account>("AccountsOdata");
    // Thêm các entity set khác n?u c?n
    return builder.GetEdmModel();
}