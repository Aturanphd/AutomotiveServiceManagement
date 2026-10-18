using Core;
using Microsoft.EntityFrameworkCore;

namespace Data;
public class ServiceDbContext(DbContextOptions<ServiceDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<ServiceRecord> Services => Set<ServiceRecord>();
    public DbSet<ServicePart> ServiceParts => Set<ServicePart>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Customer>().Property(x => x.Name).HasMaxLength(100);
        b.Entity<Customer>().Property(x => x.Phone).HasMaxLength(20);
        b.Entity<Vehicle>().HasIndex(x => x.Plate).IsUnique();
        b.Entity<Part>().HasIndex(x => x.Code).IsUnique();
        b.Entity<ServicePart>().HasIndex(x => new { x.ServiceRecordId, x.PartId }).IsUnique();
        b.Entity<Vehicle>().HasOne(x => x.Customer).WithMany(x => x.Vehicles).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<ServiceRecord>().HasOne(x => x.Vehicle).WithMany(x => x.Services).HasForeignKey(x => x.VehicleId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<ServicePart>().HasOne(x => x.ServiceRecord).WithMany(x => x.Parts).HasForeignKey(x => x.ServiceRecordId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<ServicePart>().HasOne(x => x.Part).WithMany().HasForeignKey(x => x.PartId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Vehicle>().ToTable(t => t.HasCheckConstraint("CK_Vehicle_Mileage", "Mileage >= 0 AND Mileage <= 3000000"));
        b.Entity<Part>().ToTable(t => t.HasCheckConstraint("CK_Part_Values", "Stock >= 0 AND Stock <= 1000000 AND PriceCents >= 0 AND MinimumStock >= 0"));
        b.Entity<ServiceRecord>().ToTable(t => t.HasCheckConstraint("CK_Service_Values", "LaborCents >= 0 AND Mileage >= 0 AND Status IN (0,1,2)"));
        b.Entity<ServicePart>().ToTable(t => t.HasCheckConstraint("CK_ServicePart_Values", "Quantity > 0 AND Quantity <= 10000 AND UnitPriceCents >= 0"));
        foreach (var type in b.Model.GetEntityTypes()) b.Entity(type.ClrType).Property(nameof(Entity.Version)).IsConcurrencyToken();
        b.Entity<ServiceRecord>().Ignore(x => x.TotalCents).Ignore(x => x.StatusText);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Entity>().Where(x => x.State is EntityState.Modified or EntityState.Added)) entry.Entity.Version = Guid.NewGuid();
        return base.SaveChangesAsync(cancellationToken);
    }
}
