using Core;
using Microsoft.AspNetCore.Mvc;
namespace Web.UI.Controllers;
public class HomeController(IDashboardService dashboard) : Controller
{
    public async Task<IActionResult> Index() => View(await dashboard.GetAsync());
    public IActionResult Help() => View();
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() { Response.StatusCode = 500; return View("Message", "İşlem tamamlanamadı. Sayfayı yenileyip tekrar deneyiniz. Sorun sürerse uygulama günlüğünü kontrol ediniz."); }
    public IActionResult Status(int code) { Response.StatusCode = code; return View("Message", code == 404 ? "Aradığınız kayıt veya sayfa bulunamadı." : "İstek işlenemedi. Formu yeniden açıp tekrar deneyiniz."); }
}
