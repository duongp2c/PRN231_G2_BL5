using PRN231_API.DAO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ManageGrade}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Ensure this line is present
});

app.Run();
