using System.ComponentModel.DataAnnotations;

namespace Core;
public class EditForm
{
    public int Id { get; set; }
    public Guid Version { get; set; }
}
public class CustomerForm : EditForm
{
    [Required(ErrorMessage = "Ad soyad giriniz."), StringLength(100, MinimumLength = 2, ErrorMessage = "Ad soyad 2-100 karakter olmalı."), Display(Name = "Ad soyad")]
    public string Name { get; set; } = "";
    [Required(ErrorMessage = "Telefon giriniz."), RegularExpression(@"\+?[0-9 ()\-]{10,20}", ErrorMessage = "10-20 karakterli geçerli bir telefon giriniz."), Display(Name = "Telefon")]
    public string Phone { get; set; } = "";
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz."), StringLength(150, ErrorMessage = "E-posta en fazla 150 karakter olabilir."), Display(Name = "E-posta (isteğe bağlı)")]
    public string? Email { get; set; }
}
public class VehicleForm : EditForm, IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "Müşteri seçiniz."), Display(Name = "Müşteri")]
    public int CustomerId { get; set; }
    [Required(ErrorMessage = "Plaka giriniz."), StringLength(15, ErrorMessage = "Plaka en fazla 15 karakter olabilir."), RegularExpression(@"[A-Za-z0-9 \-]{3,15}", ErrorMessage = "Plaka 3-15 harf/rakam içermeli."), Display(Name = "Plaka")]
    public string Plate { get; set; } = "";
    [Required(ErrorMessage = "Marka giriniz."), StringLength(50, ErrorMessage = "Marka en fazla 50 karakter olabilir."), Display(Name = "Marka")]
    public string Brand { get; set; } = "";
    [Required(ErrorMessage = "Model giriniz."), StringLength(50, ErrorMessage = "Model en fazla 50 karakter olabilir."), Display(Name = "Model")]
    public string Model { get; set; } = "";
    [Display(Name = "Model yılı")]
    public int Year { get; set; } = DateTime.Today.Year;
    [Range(0, 3000000, ErrorMessage = "Kilometre 0-3.000.000 arasında olmalı."), Display(Name = "Kilometre")]
    public int Mileage { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Year < 1950 || Year > DateTime.Today.Year + 1) yield return new("Geçerli model yılı giriniz.", [nameof(Year)]);
    }
}
public class PartForm : EditForm
{
    [Required(ErrorMessage = "Parça kodu giriniz."), RegularExpression(@"[A-Za-z0-9\-]{2,30}", ErrorMessage = "Kod 2-30 harf/rakam veya tire olmalı."), Display(Name = "Parça kodu")]
    public string Code { get; set; } = "";
    [Required(ErrorMessage = "Parça adı giriniz."), StringLength(100, ErrorMessage = "Parça adı en fazla 100 karakter olabilir."), Display(Name = "Parça adı")]
    public string Name { get; set; } = "";
    [Range(typeof(decimal), "0", "1000000", ErrorMessage = "Fiyat 0-1.000.000 TL arasında olmalı."), Display(Name = "Birim fiyat (TL)")]
    public decimal Price { get; set; }
    [Range(0, 1000000, ErrorMessage = "Eşik 0-1.000.000 arasında olmalı."), Display(Name = "Kritik stok eşiği")]
    public int MinimumStock { get; set; } = 5;
}
public class ServiceForm : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "Araç seçiniz."), Display(Name = "Araç")]
    public int VehicleId { get; set; }
    [DataType(DataType.Date), Display(Name = "Servis tarihi")]
    public DateTime Date { get; set; } = DateTime.Today;
    [Range(0, 3000000, ErrorMessage = "Geçerli kilometre giriniz."), Display(Name = "Giriş kilometresi")]
    public int Mileage { get; set; }
    [Required(ErrorMessage = "Şikayet ve yapılacak işlemi açıklayınız."), StringLength(2000, MinimumLength = 5, ErrorMessage = "Açıklama 5-2000 karakter olmalı."), Display(Name = "Şikayet / yapılacak işlem")]
    public string Description { get; set; } = "";
    [Range(typeof(decimal), "0", "1000000", ErrorMessage = "İşçilik 0-1.000.000 TL arasında olmalı."), Display(Name = "İşçilik (TL)")]
    public decimal Labor { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Date.Date > DateTime.Today || Date.Year < 2000) yield return new("Servis tarihi 2000 yılı ile bugün arasında olmalı.", [nameof(Date)]);
    }
}
public class QuantityForm
{
    public int Id { get; set; }
    public Guid Version { get; set; }
    [Range(1, 10000, ErrorMessage = "Miktar 1-10.000 arasında olmalı.")]
    public int Quantity { get; set; } = 1;
}
public class AddServicePartForm : QuantityForm
{
    [Range(1, int.MaxValue, ErrorMessage = "Parça seçiniz.")]
    public int PartId { get; set; }
}
public record Page<T>(IReadOnlyList<T> Items, int Number, int TotalPages, string? Query);
public record Dashboard(int Customers, int Vehicles, int OpenServices, long CompletedCents, IReadOnlyList<Part> LowStock, IReadOnlyList<ServiceRecord> Recent);
public class RuleException(string message) : Exception(message);
