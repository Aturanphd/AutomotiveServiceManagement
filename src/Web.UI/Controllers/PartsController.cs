using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Web.UI.Controllers;
public class PartsController(IPartService service) : Controller
{
    public async Task<IActionResult> Index(string? q, int page = 1) => View(await service.ListAsync(q, page));
    public async Task<IActionResult> Details(int id) => await service.GetAsync(id) is { } entity ? View(entity) : NotFound();
    public async Task<IActionResult> Edit(int id = 0)
    {
        
        if (id == 0) return View(new PartForm());
        var x = await service.GetAsync(id);
        return x == null ? NotFound() : View(new PartForm { Id = x.Id, Version = x.Version, Code = x.Code, Name = x.Name, MinimumStock = x.MinimumStock, Price = x.PriceCents / 100m });
    }
    [HttpPost]
    public async Task<IActionResult> Edit(PartForm form)
    {
        if (ModelState.IsValid) try { await service.SaveAsync(form); TempData["Success"] = "Parça kaydedildi."; return RedirectToAction(nameof(Index)); } catch (RuleException e) { ModelState.AddModelError("", e.Message); }
        
        return View(form);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id, Guid version)
    {
        try { await service.DeleteAsync(id, version); TempData["Success"] = "Parça silindi."; return RedirectToAction(nameof(Index)); }
        catch (RuleException e) { TempData["Error"] = e.Message; return RedirectToAction(nameof(Details), new { id }); }
    }
    
    [HttpPost]
    public async Task<IActionResult> Receive(QuantityForm form)
    {
        if (!ModelState.IsValid) TempData["Error"] = "Miktar 1-10.000 arasında olmalı.";
        else try { await service.ReceiveAsync(form); TempData["Success"] = "Stok girişi kaydedildi."; } catch (RuleException e) { TempData["Error"] = e.Message; }
        return RedirectToAction(nameof(Details), new { id = form.Id });
    }

}
