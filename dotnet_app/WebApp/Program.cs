using WebApp;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.ConfigureLogging();

builder.Services.AddHttpClient<ValorantSkinsDBService>();
builder.Services.AddSingleton<ValorantAuthService>();
builder.Services.AddScoped<ValorantStoreService>();

if (!builder.Configuration.GetValue<bool>("USE_FAKE_DATA"))
    builder.Services.AddScoped<IValorantService, ValorantService>();
else
    builder.Services.AddScoped<IValorantService, ValorantFakeDataService>();

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
