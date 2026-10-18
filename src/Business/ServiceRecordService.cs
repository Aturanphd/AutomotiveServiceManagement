using Core;
using Data;
using Microsoft.EntityFrameworkCore;
namespace Business;
public class ServiceRecordService(ServiceDbContext db) : ServiceBase(db), IServiceRecordService
{
    public Task<Page<ServiceRecord>> ListAsync(string? q, int page)
    {
        var query = Db.Services.AsNoTracking().Include(x => x.Vehicle).ThenInclude(x => x.Customer).Include(x => x.Parts).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) { var plate = VehicleService.Normalize(q); query = query.Where(x => x.Vehicle.Plate.Contains(plate) || x.Description.Contains(q)); }
        return Paginate(query.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id), page, q);
    }
    public Task<ServiceRecord?> GetAsync(int id) => Db.Services.AsNoTracking().Include(x => x.Vehicle).ThenInclude(x => x.Customer).Include(x => x.Parts).ThenInclude(x => x.Part).SingleOrDefaultAsync(x => x.Id == id);
    public Task<List<Vehicle>> VehiclesAsync() => Db.Vehicles.AsNoTracking().Include(x => x.Customer).OrderBy(x => x.Plate).ToListAsync();
    public Task<List<Part>> PartsAsync() => Db.Parts.AsNoTracking().Where(x => x.Stock > 0).OrderBy(x => x.Name).ToListAsync();
    public async Task<int> CreateAsync(ServiceForm f)
    {
        f.Description = f.Description?.Trim() ?? "";
        Validate(f); var labor = Cents(f.Labor);
        var vehicle = await Db.Vehicles.FindAsync(f.VehicleId) ?? throw new RuleException("Araç bulunamadı.");
        if (f.Mileage < vehicle.Mileage) throw new RuleException($"Kilometre en az {vehicle.Mileage:N0} olmalı.");
        if (await Db.Services.AnyAsync(x => x.VehicleId == f.VehicleId && x.Status == ServiceStatus.Open)) throw new RuleException("Bu aracın açık servisi var. Önce mevcut servisi tamamlayınız veya iptal ediniz.");
        var record = new ServiceRecord { VehicleId = f.VehicleId, Date = f.Date.Date, Mileage = f.Mileage, Description = f.Description.Trim(), LaborCents = labor };
        // Even unchanged mileage touches the vehicle, serializing concurrent service creation.
        vehicle.Mileage = f.Mileage; Db.Entry(vehicle).Property(x => x.Mileage).IsModified = true;
        Db.Services.Add(record); await SaveAsync(); return record.Id;
    }
    public async Task AddPartAsync(AddServicePartForm f)
    {
        Validate(f);
        await using var transaction = await Db.Database.BeginTransactionAsync();
        var record = await Db.Services.Include(x => x.Parts).SingleOrDefaultAsync(x => x.Id == f.Id) ?? throw new RuleException("Servis bulunamadı.");
        CheckVersion(record, f.Version); EnsureOpen(record);
        var part = await Db.Parts.FindAsync(f.PartId) ?? throw new RuleException("Parça bulunamadı.");
        if (part.Stock < f.Quantity) throw new RuleException($"Yetersiz stok. Kullanılabilir: {part.Stock} adet.");
        var line = record.Parts.SingleOrDefault(x => x.PartId == f.PartId);
        if (line != null) throw new RuleException("Bu parça servise eklenmiş. Gerekirse servisi iptal edip doğru miktarla yeniden açınız.");
        part.Stock -= f.Quantity;
        record.Parts.Add(new ServicePart { PartId = part.Id, Quantity = f.Quantity, UnitPriceCents = part.PriceCents });
        Db.Entry(record).Property(x => x.Version).IsModified = true;
        await SaveAsync(); await transaction.CommitAsync();
    }
    public async Task ChangeStatusAsync(int id, Guid version, ServiceStatus status)
    {
        if (status is not (ServiceStatus.Completed or ServiceStatus.Cancelled)) throw new RuleException("Geçersiz durum.");
        await using var transaction = await Db.Database.BeginTransactionAsync();
        var record = await Db.Services.Include(x => x.Parts).ThenInclude(x => x.Part).SingleOrDefaultAsync(x => x.Id == id) ?? throw new RuleException("Servis bulunamadı.");
        CheckVersion(record, version); EnsureOpen(record);
        if (status == ServiceStatus.Cancelled)
            foreach (var line in record.Parts)
            {
                if ((long)line.Part.Stock + line.Quantity > 1000000) throw new RuleException("İade stok sınırını aşacak. Stokları kontrol ediniz.");
                line.Part.Stock += line.Quantity;
            }
        record.Status = status; await SaveAsync(); await transaction.CommitAsync();
    }
    private static void EnsureOpen(ServiceRecord r)
    {
        if (r.Status != ServiceStatus.Open) throw new RuleException("Yalnızca açık servis değiştirilebilir. Bu servis daha önce kapatılmış.");
    }
}
