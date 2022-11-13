using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.User;
using WebApplication2.Models.Travel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// https://stackoverflow.com/questions/60322252/asp-net-core-web-app-di-error-some-services-are-not-able-to-be-constructed-er
builder.Services.AddScoped<IUserRepository, MockUserRepository>();//Dependancy injection for using UserModel
builder.Services.AddScoped<ITravelRepository, TravelRepository>();//Dependancy injection for using TravelModel
builder.Services.AddScoped<ICorrelationIDGenerator, CorrelationIdGenerator>();  //Dependancy injection for CorrelationIdGenerator
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Use CorrelationID and Exception middleware
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();