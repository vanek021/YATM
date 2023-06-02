using Hangfire;
using YATM.Core.Extensions;
using YATM.Core.Models;
using YATM.Data;
using YATM.Models.Entities;
using YATM.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddBasePgsqlContext<AppDbContext>(connectionString);

builder.Services.RegisterInjectableTypesFromAssemblies(typeof(Program), typeof(AppDbContext));

builder.Services.AddApplicationIdentity<AppDbContext>();

builder.Services.AddAutoMapper(typeof(Program), typeof(AppDbContext), typeof(User));

builder.Services.AddFileSystemBucketStorage(builder.Environment.WebRootPath, "default");

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.LogoutPath = "/Identity/Account/Logout";
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddAntDesign();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped<IAuthenticationStateAccessor, AuthenticationStateAccessor>();
builder.Services.AddScoped<ApplicationContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();
