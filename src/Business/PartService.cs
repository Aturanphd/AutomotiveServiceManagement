using Core;
using Data;
using Microsoft.EntityFrameworkCore;
namespace Business;
public class PartService(ServiceDbContext db) : ServiceBase(db), IPartService
{
    public Task<Page<Part>> ListAsync(string? q, int page)
    {
        var query = Db.Parts.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Name.Contains(q) || x.Code.Contains(q));
        return Paginate(query.OrderBy(x => x.Code), page, q);
    }
    public Task<Part?> GetAsync(int id) => Db.Parts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    public async Task SaveAsync(PartForm f)
    {
        f.Name = f.Name?.Trim() ?? "";
        Validate(f); var price = Cents(f.Price); var code = f.Code.Trim().ToUpperInvariant();
        if (await Db.Parts.AnyAsync(x => x.Code == code && x.Id != f.Id)) throw new RuleException("Bu parça kodu zaten kayıtlı.");
        var entity = f.Id == 0 ? new Part() : await Db.Parts.FindAsync(f.Id) ?? throw new RuleException("Parça bulunamadı.");
        if (f.Id != 0) CheckVersion(entity, f.Version); else Db.Parts.Add(entity);
        entity.Code = code; entity.Name = f.Name.Trim(); entity.PriceCents = price; entity.MinimumStock = f.MinimumStock;
        await SaveAsync();
    }
    public async Task ReceiveAsync(QuantityForm f)
    {
        Validate(f);
        var entity = await Db.Parts.FindAsync(f.Id) ?? throw new RuleException("Parça bulunamadı.");
        CheckVersion(entity, f.Version);
        if ((long)entity.Stock + f.Quantity > 1000000) throw new RuleException("Stok 1.000.000 adedi aşamaz.");
        entity.Stock += f.Quantity; await SaveAsync();
    }
    public async Task DeleteAsync(int id, Guid version)
    {
        var entity = await Db.Parts.FindAsync(id) ?? throw new RuleException("Parça bulunamadı.");
        CheckVersion(entity, version);
        if (entity.Stock != 0 || await Db.ServiceParts.AnyAsync(x => x.PartId == id)) throw new RuleException("Stok bulunan veya serviste kullanılan parça silinemez.");
        Db.Parts.Remove(entity); await SaveAsync();
    }
}
