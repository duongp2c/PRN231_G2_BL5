
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PRN231_API.DAO;
using PRN231_API.Models;
using PRN231_API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddDbContext<SchoolDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentManagement")));
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

builder.Services.AddScoped<EmailDAO>();
builder.Services.AddScoped<JwtTokenService>();
//builder.Services.AddScoped<IAccountService, AccountDAO>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration["Redis:ConnectionString"];
//    options.InstanceName = "SampleInstance";
//});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(configuration);
});


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuer = true,
        ValidateAudience = true
    };
});

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("Student", policy => policy.RequireClaim(ClaimTypes.Role, "Student"));
    x.AddPolicy("Teacher", policy => policy.RequireClaim(ClaimTypes.Role, "Teacher"));
    x.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CORSPolicy");

app.UseODataBatching();

app.MapControllers();

app.Run();
IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Account>("AccountsOdata");
    // Th�m c�c entity set kh�c n?u c?n
    return builder.GetEdmModel();
}