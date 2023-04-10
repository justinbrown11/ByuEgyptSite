using ByuEgyptSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
connectionString = connectionString.Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"));
connectionString = connectionString.Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"));
connectionString = connectionString.Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"));
connectionString = connectionString.Replace("${DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 15;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoleManager<RoleManager<IdentityRole>>();

var roleManager = builder.Services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();

//var result = await roleManager.CreateAsync(new IdentityRole("Administrator"));
//if (result.Succeeded)
//{
//    Console.WriteLine("Administrator role created");
//}

//else
//{
//    Console.WriteLine("Failed to create role 'Manager':");
//    foreach (var error in result.Errors)
//    {
//        Console.WriteLine(error.Description);
//    }
//}

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_AUTH_CLIENT_ID");
    options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_AUTH_CLIENT_SECRET");
    options.CallbackPath = "/signin-google";
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    // requires using Microsoft.AspNetCore.Http;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});
builder.Services.AddControllersWithViews();


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
app.UseHsts();

app.Use(async (context, next) =>
{
    // Set content security policy header
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'");

    // Call next middleware in pipeline
    await next();
});

app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
//    var user = await userManager.FindByNameAsync("justinjoelbrown@gmail.com");
//    if (user != null)
//    {
//        await userManager.AddToRoleAsync(user, "Administrator");
//    }
//}

app.Run();
