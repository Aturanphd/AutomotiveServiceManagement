using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Web.UI.Controllers;
public class CustomersController(ICustomerService service) : Controller
{
    public async Task<IActionResult> Index(string? q, int page = 1) => View(await service.ListAsync(q, page));
    public async Task<IActionResult> Details(int id) => await service.GetAsync(id) is { } entity ? View(entity) : NotFound();
    public async Task<IActionResult> Edit(int id = 0)
    {
        
        if (id == 0) return View(new CustomerForm());
        var x = await service.GetAsync(id);
        return x == null ? NotFound() : View(new CustomerForm { Id = x.Id, Version = x.Version, Name = x.Name, Phone = x.Phone, Email = x.Email });
    }
    [HttpPost]
    public async Task<IActionResult> Edit(CustomerForm form)
    {
        if (ModelState.IsValid) try { await service.SaveAsync(form); TempData["Success"] = "Müşteri kaydedildi."; return RedirectToAction(nameof(Index)); } catch (RuleException e) { ModelState.AddModelError("", e.Message); }
        
        return View(form);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id, Guid version)
    {
        try { await service.DeleteAsync(id, version); TempData["Success"] = "Müşteri silindi."; return RedirectToAction(nameof(Index)); }
        catch (RuleException e) { TempData["Error"] = e.Message; return RedirectToAction(nameof(Details), new { id }); }
    }
    
}
