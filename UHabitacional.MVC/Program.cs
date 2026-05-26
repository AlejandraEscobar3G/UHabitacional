using UHabitacional.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// =====================================================================
// MVC
// =====================================================================
builder.Services.AddControllersWithViews();

// =====================================================================
// Sesión (almacenamos JWT y usuario logueado en TempData/Session)
// =====================================================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".UHabitacional.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});

builder.Services.AddHttpContextAccessor();

// =====================================================================
// HttpClient hacia la API
// =====================================================================
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                 ?? "http://localhost:5000/";

builder.Services.AddHttpClient<IApiClient, ApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// =====================================================================
// Helpers
// =====================================================================
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = builder.Build();

// =====================================================================
// Pipeline
// =====================================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
