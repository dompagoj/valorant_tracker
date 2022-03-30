using WebApp;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.ConfigureLogging();

builder.Services.AddHttpClient<ValorantSkinsDBService>();
builder.Services.AddSingleton<ValorantAuthService>();
builder.Services.AddScoped<ValorantStoreService>();
builder.Services.AddScoped<ValorantService>();

builder.Services.AddNodeServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
