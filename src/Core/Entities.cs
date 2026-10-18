namespace Core;

public abstract class Entity
{
    public int Id { get; set; }
    public Guid Version { get; set; } = Guid.NewGuid();
}
public class Customer : Entity
{
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string? Email { get; set; }
    public List<Vehicle> Vehicles { get; set; } = [];
}
public class Vehicle : Entity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string Plate { get; set; } = "";
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public int Year { get; set; }
    public int Mileage { get; set; }
    public List<ServiceRecord> Services { get; set; } = [];
}
public enum ServiceStatus { Open, Completed, Cancelled }
public class ServiceRecord : Entity
{
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public DateTime Date { get; set; }
    public int Mileage { get; set; }
    public string Description { get; set; } = "";
    public int LaborCents { get; set; }
    public ServiceStatus Status { get; set; }
    public List<ServicePart> Parts { get; set; } = [];
    public long TotalCents => LaborCents + Parts.Sum(p => (long)p.Quantity * p.UnitPriceCents);
    public string StatusText => Status switch { ServiceStatus.Open => "Açık", ServiceStatus.Completed => "Tamamlandı", _ => "İptal" };
}
public class Part : Entity
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public int PriceCents { get; set; }
    public int Stock { get; set; }
    public int MinimumStock { get; set; }
}
public class ServicePart : Entity
{
    public int ServiceRecordId { get; set; }
    public ServiceRecord ServiceRecord { get; set; } = null!;
    public int PartId { get; set; }
    public Part Part { get; set; } = null!;
    public int Quantity { get; set; }
    public int UnitPriceCents { get; set; }
}
