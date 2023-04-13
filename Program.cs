using ByuEgyptSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.UI.Services;
using ByuEgyptSite;
using Microsoft.ML.OnnxRuntime;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
DotNetEnv.Env.Load();

// Get db connection string and replace placeholders with .env credentials
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
connectionString = connectionString.Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"));
connectionString = connectionString.Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"));
connectionString = connectionString.Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"));
connectionString = connectionString.Replace("${DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS"));
// Add db context with connection string using PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add AspNetCore.Identity default service
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    // Set strong password policies
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 15;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<IdentityRole>() // add roles for RBAC
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure password hashing
builder.Services.Configure<PasswordHasherOptions>(options =>
{
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
});

// Add user and role managers to scope
builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

// Add authentication, including third party Google
builder.Services.AddAuthentication().AddGoogle(options =>
{
    // Grab google api creds from .env
    options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_AUTH_CLIENT_ID");
    options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_AUTH_CLIENT_SECRET");

    // Set callback path
    options.CallbackPath = "/signin-google";
});

// Configure SendGrid for authentication emails
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// configure cookies
builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromDays(1);
    o.SlidingExpiration = true;
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
{
    o.TokenLifespan = TimeSpan.FromHours(3);
});

// Configure cookie policies
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    // requires using Microsoft.AspNetCore.Http;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Enforces HSTS
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

// Adds MVC
builder.Services.AddControllersWithViews();

//line below is for Supervised Learning Model experimentation
builder.Services.AddSwaggerGen();


//line below is for Supervised Learning Model
//added this below
builder.Services.AddSingleton<InferenceSession>(
  new InferenceSession("myOnnxFile1.onnx")
);

// Build the app
var app = builder.Build();

//added this for Supervised Learning Model
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// THIS IS NOT YET WORKING PROPERLY (won't login)
// Seeds roles and create an admin user
using (var scope = app.Services.CreateScope())
{
    try
    {
        // Create user and role managers
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // If Administrator role does not yet exist, create one
        if (!await roleManager.RoleExistsAsync("Administrator"))
        {
            await roleManager.CreateAsync(new IdentityRole("Administrator"));
        }

        // If Researcher role does not yet exist, create one
        if (!await roleManager.RoleExistsAsync("Researcher"))
        {
            await roleManager.CreateAsync(new IdentityRole("Researcher"));
        }

        // Check if admin user exists, and create if it doesn't
        var user = await userManager.FindByNameAsync("admin");

        // If admin user was not already found, create one
        if (user == null)
        {
            // Create new user object
            user = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true,
            };

            // Hash temporary password
            var tempPassword = Environment.GetEnvironmentVariable("TEMP_PASS");
            var passwordHasher = new PasswordHasher<IdentityUser>();
            var hashedPassword = passwordHasher.HashPassword(user, tempPassword);
            user.PasswordHash = hashedPassword;

            // Create the user
            var result = await userManager.CreateAsync(user);

            // If successful
            if (result.Succeeded)
            {
                // Give admin role
                await userManager.AddToRoleAsync(user, "Administrator");
            }
        }
    }

    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}

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

// Use HTTPS redirection and HSTS
app.UseHttpsRedirection();
app.UseHsts();

// Configure CSP headers
app.Use(async (context, next) =>
{
    // Set content security policy header
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'");

    // Call next middleware in pipeline
    await next();
});

// Use static files
app.UseStaticFiles();

// Use routing
app.UseRouting();

// Enable cookie policies
app.UseCookiePolicy();

// Authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Route pattern
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
