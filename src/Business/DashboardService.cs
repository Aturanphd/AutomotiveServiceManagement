using Core;
using Data;
using Microsoft.EntityFrameworkCore;
namespace Business;
public class DashboardService(ServiceDbContext db) : IDashboardService
{
    public async Task<Dashboard> GetAsync()
    {
        var labor = await db.Services.Where(x => x.Status == ServiceStatus.Completed).SumAsync(x => (long)x.LaborCents);
        var parts = await db.ServiceParts.Where(x => x.ServiceRecord.Status == ServiceStatus.Completed).SumAsync(x => (long)x.Quantity * x.UnitPriceCents);
        return new(await db.Customers.CountAsync(), await db.Vehicles.CountAsync(), await db.Services.CountAsync(x => x.Status == ServiceStatus.Open), labor + parts,
            await db.Parts.AsNoTracking().Where(x => x.Stock <= x.MinimumStock).OrderBy(x => x.Stock).Take(10).ToListAsync(),
            await db.Services.AsNoTracking().Include(x => x.Vehicle).Include(x => x.Parts).OrderByDescending(x => x.Id).Take(5).ToListAsync());
    }
}
