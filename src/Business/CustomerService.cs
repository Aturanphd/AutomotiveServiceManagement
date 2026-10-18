using Core;
using Data;
using Microsoft.EntityFrameworkCore;
namespace Business;
public class CustomerService(ServiceDbContext db) : ServiceBase(db), ICustomerService
{
    public Task<Page<Customer>> ListAsync(string? q, int page)
    {
        var query = Db.Customers.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Name.Contains(q) || x.Phone.Contains(q));
        return Paginate(query.OrderBy(x => x.Name), page, q);
    }
    public Task<Customer?> GetAsync(int id) => Db.Customers.AsNoTracking().Include(x => x.Vehicles).SingleOrDefaultAsync(x => x.Id == id);
    public async Task SaveAsync(CustomerForm f)
    {
        f.Name = f.Name?.Trim() ?? "";
        Validate(f);
        var entity = f.Id == 0 ? new Customer() : await Db.Customers.FindAsync(f.Id) ?? throw new RuleException("Müşteri bulunamadı.");
        if (f.Id != 0) CheckVersion(entity, f.Version); else Db.Customers.Add(entity);
        entity.Name = f.Name.Trim(); entity.Phone = f.Phone.Trim(); entity.Email = f.Email?.Trim();
        await SaveAsync();
    }
    public async Task DeleteAsync(int id, Guid version)
    {
        var entity = await Db.Customers.FindAsync(id) ?? throw new RuleException("Müşteri bulunamadı.");
        CheckVersion(entity, version);
        if (await Db.Vehicles.AnyAsync(x => x.CustomerId == id)) throw new RuleException("Bu müşteriye bağlı araçlar var. Önce araç ilişkilerini kontrol ediniz.");
        Db.Customers.Remove(entity); await SaveAsync();
    }
}
