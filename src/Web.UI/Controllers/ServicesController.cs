using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Web.UI.Controllers;
public class ServicesController(IServiceRecordService service) : Controller
{
    public async Task<IActionResult> Index(string? q, int page = 1) => View(await service.ListAsync(q, page));
    public async Task<IActionResult> Details(int id)
    {
        var entity = await service.GetAsync(id); if (entity == null) return NotFound();
        ViewBag.PartSelectionInvalid = TempData["PartSelectionInvalid"] is true;
        ViewBag.Parts = (await service.PartsAsync()).Select(x => new SelectListItem($"{x.Code} · {x.Name} · {x.Stock} adet", x.Id.ToString())).ToList();
        return View(entity);
    }
    private async Task Options() => ViewBag.Vehicles = (await service.VehiclesAsync()).Select(x => new SelectListItem($"{x.Plate} · {x.Customer.Name} · {x.Mileage:N0} km", x.Id.ToString())).ToList();
    public async Task<IActionResult> Create(int vehicleId = 0)
    {
        await Options(); var vehicle = (await service.VehiclesAsync()).SingleOrDefault(x => x.Id == vehicleId);
        return View(new ServiceForm { VehicleId = vehicleId, Mileage = vehicle?.Mileage ?? 0 });
    }
    [HttpPost]
    public async Task<IActionResult> Create(ServiceForm form)
    {
        if (ModelState.IsValid) try { var id = await service.CreateAsync(form); TempData["Success"] = "Servis açıldı. Kullanılan parçaları ekleyebilirsiniz."; return RedirectToAction(nameof(Details), new { id }); } catch (RuleException e) { ModelState.AddModelError("", e.Message); }
        await Options(); return View(form);
    }
    [HttpPost]
    public async Task<IActionResult> AddPart(AddServicePartForm form)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = string.Join(" ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).Distinct());
            TempData["PartSelectionInvalid"] = ModelState.TryGetValue(nameof(form.PartId), out var field) && field.Errors.Count > 0;
        }
        else try { await service.AddPartAsync(form); TempData["Success"] = "Parça eklendi ve stok güncellendi."; } catch (RuleException e) { TempData["Error"] = e.Message; }
        return RedirectToAction(nameof(Details), new { id = form.Id });
    }
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id, Guid version, ServiceStatus status)
    {
        try { await service.ChangeStatusAsync(id, version, status); TempData["Success"] = status == ServiceStatus.Cancelled ? "Servis iptal edildi; parçalar stoğa iade edildi." : "Servis tamamlandı."; } catch (RuleException e) { TempData["Error"] = e.Message; }
        return RedirectToAction(nameof(Details), new { id });
    }
}
