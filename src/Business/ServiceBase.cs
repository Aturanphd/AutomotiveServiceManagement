using System.ComponentModel.DataAnnotations;
using Core;
using Data;
using Microsoft.EntityFrameworkCore;
namespace Business;
public abstract class ServiceBase(ServiceDbContext db)
{
    protected ServiceDbContext Db { get; } = db;
    protected static void Validate(object form)
    {
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(form, new ValidationContext(form), errors, true)) throw new RuleException(string.Join(" ", errors.Select(x => x.ErrorMessage)));
    }
    protected static void CheckVersion(Entity entity, Guid version)
    {
        if (entity.Version != version) throw new RuleException("Kayıt başka bir işlemle değişti. Sayfayı yenileyip tekrar deneyiniz.");
    }
    protected async Task SaveAsync()
    {
        try { await Db.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException) { throw new RuleException("Kayıt değişti veya silindi. Sayfayı yenileyiniz."); }
        catch (DbUpdateException) { throw new RuleException("İşlem kaydedilemedi. Tekrarlanan kod/plaka, bağlı kayıt veya stok sınırını kontrol ediniz."); }
    }
    protected static int Cents(decimal value)
    {
        if (decimal.Round(value, 2) != value) throw new RuleException("Para tutarını en fazla iki ondalık basamakla giriniz.");
        return checked((int)(value * 100));
    }
    protected static async Task<Page<T>> Paginate<T>(IQueryable<T> query, int page, string? q)
    {
        var count = await query.CountAsync();
        var pages = Math.Max(1, (int)Math.Ceiling(count / 10d));
        page = Math.Clamp(page, 1, pages);
        return new(await query.Skip((page - 1) * 10).Take(10).ToListAsync(), page, pages, q);
    }
}
