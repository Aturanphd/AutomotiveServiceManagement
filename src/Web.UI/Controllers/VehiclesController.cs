using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Web.UI.Controllers;
public class VehiclesController(IVehicleService service) : Controller
{
    public async Task<IActionResult> Index(string? q, int page = 1) => View(await service.ListAsync(q, page));
    public async Task<IActionResult> Details(int id) => await service.GetAsync(id) is { } entity ? View(entity) : NotFound();
    public async Task<IActionResult> Edit(int id = 0)
    {
        ViewBag.Customers = new SelectList(await service.CustomersAsync(), "Id", "Name");
        if (id == 0) return View(new VehicleForm());
        var x = await service.GetAsync(id);
        return x == null ? NotFound() : View(new VehicleForm { Id = x.Id, Version = x.Version, CustomerId = x.CustomerId, Plate = x.Plate, Brand = x.Brand, Model = x.Model, Year = x.Year, Mileage = x.Mileage });
    }
    [HttpPost]
    public async Task<IActionResult> Edit(VehicleForm form)
    {
        if (ModelState.IsValid) try { await service.SaveAsync(form); TempData["Success"] = "Araç kaydedildi."; return RedirectToAction(nameof(Index)); } catch (RuleException e) { ModelState.AddModelError("", e.Message); }
        ViewBag.Customers = new SelectList(await service.CustomersAsync(), "Id", "Name");
        return View(form);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id, Guid version)
    {
        try { await service.DeleteAsync(id, version); TempData["Success"] = "Araç silindi."; return RedirectToAction(nameof(Index)); }
        catch (RuleException e) { TempData["Error"] = e.Message; return RedirectToAction(nameof(Details), new { id }); }
    }
    
}
