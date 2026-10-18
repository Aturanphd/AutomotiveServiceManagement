using Core;
using Microsoft.EntityFrameworkCore;
namespace Data;
public static class SeedData
{
    public static async Task InitializeAsync(ServiceDbContext db)
    {
        await AddMissingDemoPartsAsync(db);
        if (await db.Customers.AnyAsync()) return;
        var customer = new Customer { Name = "Deniz Yılmaz", Phone = "0532 000 11 22", Email = "deniz@example.com" };
        var vehicle = new Vehicle { Customer = customer, Plate = "34ABC123", Brand = "Renault", Model = "Clio", Year = 2020, Mileage = 65000 };
        var oil = new Part { Code = "YAG-5W30", Name = "Motor yağı 5W-30 (1 litre)", PriceCents = 45000, Stock = 16, MinimumStock = 5 };
        db.Customers.AddRange(customer, new Customer { Name = "Ece Kaya", Phone = "0533 000 33 44" });
        db.Vehicles.Add(vehicle);
        db.Parts.AddRange(oil, new Part { Code = "FLT-001", Name = "Yağ filtresi", PriceCents = 28000, Stock = 3, MinimumStock = 5 });
        db.Services.AddRange(new ServiceRecord { Vehicle = vehicle, Date = DateTime.Today, Mileage = 65000, Description = "Periyodik bakım ve fren kontrolü", LaborCents = 120000 }, new ServiceRecord { Vehicle = vehicle, Date = DateTime.Today.AddDays(-30), Mileage = 60000, Description = "Motor yağı değişimi", LaborCents = 80000, Status = ServiceStatus.Completed, Parts = [new ServicePart { Part = oil, Quantity = 4, UnitPriceCents = 45000 }] });
        await db.SaveChangesAsync();
    }

    // Yalnızca eksik demo kodlarını ekler; mevcut fiyat ve stoklara dokunmaz.
    private static async Task AddMissingDemoPartsAsync(ServiceDbContext db)
    {
        var catalog = new (string Code, string Name, int Price, int Stock, int Minimum)[] {
            ("FLT-002", "Hava filtresi", 35000, 15, 5),
            ("FLT-003", "Polen filtresi", 32000, 12, 4),
            ("FLT-004", "Yakıt filtresi", 65000, 10, 3),
            ("BAL-ON", "Ön fren balata takımı", 140000, 8, 3),
            ("BAL-ARK", "Arka fren balata takımı", 110000, 7, 2),
            ("BUJ-001", "Ateşleme bujisi", 25000, 24, 8),
            ("AKU-060", "Akü 60 Ah", 360000, 6, 2),
            ("ANT-001", "Antifriz (1 litre)", 22000, 20, 5),
            ("FRN-DOT4", "Fren hidroliği DOT 4 (1 litre)", 38000, 10, 3),
            ("SIL-ON", "Ön silecek takımı", 55000, 9, 3),
            ("AMP-H7", "H7 far ampulü", 18000, 18, 6),
            ("KAY-001", "Aksesuar kayışı", 75000, 8, 2)
        };
        var codes = await db.Parts.Select(x => x.Code).ToListAsync();
        foreach (var item in catalog.Where(x => !codes.Contains(x.Code)))
            db.Parts.Add(new Part { Code = item.Code, Name = item.Name, PriceCents = item.Price, Stock = item.Stock, MinimumStock = item.Minimum });
        await db.SaveChangesAsync();
    }
}
