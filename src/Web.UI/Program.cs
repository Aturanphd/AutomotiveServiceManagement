using System.Globalization;
using Business;
using Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.UI.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(o =>
{
    o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    o.ModelBinderProviders.Insert(0, new MoneyModelBinderProvider());
    var messages = o.ModelBindingMessageProvider;
    messages.SetAttemptedValueIsInvalidAccessor((value, field) => $"{field} alanına geçerli bir değer giriniz.");
    messages.SetValueMustNotBeNullAccessor(field => $"{field} alanını doldurunuz.");
    messages.SetValueIsInvalidAccessor(value => "Geçerli bir değer giriniz.");
    messages.SetValueMustBeANumberAccessor(field => $"{field} alanına sayı giriniz.");
    messages.SetMissingBindRequiredValueAccessor(field => $"{field} alanı zorunludur.");
});
var dataPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(dataPath);
builder.Services.AddDbContext<ServiceDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("ServiceDb") ?? $"Data Source={Path.Combine(dataPath, "service.db")};Foreign Keys=True"));
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<IServiceRecordService, ServiceRecordService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
var app = builder.Build();
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("tr-TR");
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("tr-TR");
app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture("tr-TR").AddSupportedCultures("tr-TR").AddSupportedUICultures("tr-TR"));
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
    try
    {
        await db.Database.MigrateAsync();
        if (builder.Configuration.GetValue("SeedDemoData", true)) await SeedData.InitializeAsync(db);
    }
    catch (Exception error)
    {
        app.Logger.LogCritical(error, "Veritabanı hazırlanamadı. Veri klasörünün yazma izinlerini ve veritabanı bağlantısını kontrol ediniz. Uygulama başlatılmadı.");
        throw;
    }
}
app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/Status", "?code={0}");
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
