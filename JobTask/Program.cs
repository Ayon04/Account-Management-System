using JobTask.Services;

var builder = WebApplication.CreateBuilder(args);

// Register UserService for DI BEFORE Build()
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CAService>();


// Add MVC controllers with views
builder.Services.AddControllersWithViews();

// Add Session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // optional, set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure middleware
app.UseStaticFiles();
app.UseRouting();

// Add session middleware BEFORE app.UseAuthorization()
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Register}/{id?}");

app.Run();
