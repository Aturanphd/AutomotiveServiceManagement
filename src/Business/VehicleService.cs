using Core;
using Data;
using Microsoft.EntityFrameworkCore;
namespace Business;
public class VehicleService(ServiceDbContext db) : ServiceBase(db), IVehicleService
{
    public Task<Page<Vehicle>> ListAsync(string? q, int page)
    {
        var query = Db.Vehicles.AsNoTracking().Include(x => x.Customer).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) { var plate = Normalize(q); query = query.Where(x => x.Plate.Contains(plate) || x.Brand.Contains(q) || x.Customer.Name.Contains(q)); }
        return Paginate(query.OrderBy(x => x.Plate), page, q);
    }
    public Task<Vehicle?> GetAsync(int id) => Db.Vehicles.AsNoTracking().Include(x => x.Customer).Include(x => x.Services.OrderByDescending(s => s.Date)).SingleOrDefaultAsync(x => x.Id == id);
    public Task<List<Customer>> CustomersAsync() => Db.Customers.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
    public static string Normalize(string value) => string.Concat(value.Where(char.IsLetterOrDigit)).ToUpperInvariant();
    public async Task SaveAsync(VehicleForm f)
    {
        f.Brand = f.Brand?.Trim() ?? "";
        f.Model = f.Model?.Trim() ?? "";
        Validate(f);
        var plate = Normalize(f.Plate);
        if (plate.Length < 3) throw new RuleException("Plaka en az üç harf veya rakam içermelidir.");
        if (!await Db.Customers.AnyAsync(x => x.Id == f.CustomerId)) throw new RuleException("Seçilen müşteri bulunamadı.");
        if (await Db.Vehicles.AnyAsync(x => x.Plate == plate && x.Id != f.Id)) throw new RuleException("Bu plaka zaten kayıtlı.");
        var entity = f.Id == 0 ? new Vehicle() : await Db.Vehicles.FindAsync(f.Id) ?? throw new RuleException("Araç bulunamadı.");
        if (f.Id != 0)
        {
            CheckVersion(entity, f.Version);
            if (f.Mileage < entity.Mileage) throw new RuleException("Kilometre mevcut değerden düşük olamaz.");
            if (f.CustomerId != entity.CustomerId && await Db.Services.AnyAsync(x => x.VehicleId == f.Id)) throw new RuleException("Servis geçmişi olan aracın müşterisi değiştirilemez.");
        }
        else Db.Vehicles.Add(entity);
        entity.CustomerId = f.CustomerId; entity.Plate = plate; entity.Brand = f.Brand.Trim(); entity.Model = f.Model.Trim(); entity.Year = f.Year; entity.Mileage = f.Mileage;
        await SaveAsync();
    }
    public async Task DeleteAsync(int id, Guid version)
    {
        var entity = await Db.Vehicles.FindAsync(id) ?? throw new RuleException("Araç bulunamadı.");
        CheckVersion(entity, version);
        if (await Db.Services.AnyAsync(x => x.VehicleId == id)) throw new RuleException("Servis geçmişi bulunan araç silinemez.");
        Db.Vehicles.Remove(entity); await SaveAsync();
    }
}
