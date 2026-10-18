using Business;
using Core;
using Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var passed = 0;
async Task Check(string name, Func<Task> test)
{
    await test(); passed++; Console.WriteLine($"PASS {passed:D2}: {name}");
}
void Assert(bool value) { if (!value) throw new Exception("Assertion failed"); }
async Task Reject(Func<Task> action)
{
    try { await action(); } catch (RuleException) { return; }
    throw new Exception("Expected business rule rejection");
}
await using var connection = new SqliteConnection("Data Source=:memory:;Foreign Keys=True");
await connection.OpenAsync();
var options = new DbContextOptionsBuilder<ServiceDbContext>().UseSqlite(connection).Options;
ServiceDbContext Context() => new(options);
await using (var db = Context()) { await db.Database.MigrateAsync(); }

await Check("Database migration and repeatable demo seed", async () => {
    await using var db = Context(); await SeedData.InitializeAsync(db); await SeedData.InitializeAsync(db);
    Assert(await db.Customers.CountAsync() == 2 && await db.Services.CountAsync() == 2);
});
int customerId = 0, vehicleId = 0, partId = 0, serviceId = 0;
await Check("Customer create and search", async () => {
    await using var db = Context(); var service = new CustomerService(db);
    await service.SaveAsync(new() { Name = "Test Müşteri", Phone = "05320000000", Email = "test@example.com" });
    var page = await service.ListAsync("Test", 1); Assert(page.Items.Count == 1); customerId = page.Items[0].Id;
});
await Check("Customer validation rejects invalid email/phone", async () => {
    await using var db = Context(); await Reject(() => new CustomerService(db).SaveAsync(new() { Name = "Test", Phone = "abc", Email = "bad" }));
    await Reject(() => new CustomerService(db).SaveAsync(new() { Name = " a ", Phone = "05320000000" }));
});
await Check("Customer update", async () => {
    await using var db = Context(); var s = new CustomerService(db); var c = (await s.GetAsync(customerId))!;
    await s.SaveAsync(new() { Id = c.Id, Version = c.Version, Name = "Test Güncel", Phone = c.Phone }); Assert((await s.GetAsync(c.Id))!.Name == "Test Güncel");
});
await Check("Stale customer version rejected", async () => {
    await using var db = Context(); await Reject(() => new CustomerService(db).SaveAsync(new() { Id = customerId, Version = Guid.NewGuid(), Name = "Stale", Phone = "05320000000" }));
});
await Check("Vehicle creation normalizes plate", async () => {
    await using var db = Context(); var s = new VehicleService(db);
    await s.SaveAsync(new() { CustomerId = customerId, Plate = "06 test 123", Brand = "Ford", Model = "Focus", Year = 2020, Mileage = 10000 });
    var list = await s.ListAsync("06test123", 1); Assert(list.Items.Count == 1); vehicleId = list.Items[0].Id; Assert(list.Items[0].Plate == "06TEST123");
});
await Check("Duplicate normalized plate rejected", async () => {
    await using var db = Context(); await Reject(() => new VehicleService(db).SaveAsync(new() { CustomerId = customerId, Plate = "06 TEST123", Brand = "Ford", Model = "Focus", Year = 2020 }));
});
await Check("Vehicle rejects nonexistent customer and future model year", async () => {
    await using var db = Context(); var s = new VehicleService(db);
    await Reject(() => s.SaveAsync(new() { CustomerId = 99999, Plate = "06 XX 123", Brand = "Ford", Model = "Focus", Year = 2020 }));
    await Reject(() => s.SaveAsync(new() { CustomerId = customerId, Plate = "06 XX 123", Brand = "Ford", Model = "Focus", Year = 2200 }));
    await Reject(() => s.SaveAsync(new() { CustomerId = customerId, Plate = "---", Brand = "Ford", Model = "Focus", Year = 2020 }));
});
await Check("Customer with vehicle cannot be deleted", async () => {
    await using var db = Context(); var s = new CustomerService(db); var c = (await s.GetAsync(customerId))!; await Reject(() => s.DeleteAsync(c.Id, c.Version));
});
await Check("Vehicle mileage rollback rejected", async () => {
    await using var db = Context(); var s = new VehicleService(db); var v = (await s.GetAsync(vehicleId))!;
    await Reject(() => s.SaveAsync(new() { Id = v.Id, Version = v.Version, CustomerId = v.CustomerId, Plate = v.Plate, Brand = v.Brand, Model = v.Model, Year = v.Year, Mileage = 9999 }));
});
await Check("Part creation starts at zero stock", async () => {
    await using var db = Context(); var s = new PartService(db);
    await s.SaveAsync(new() { Code = "test-01", Name = "Test parça", Price = 125.50m, MinimumStock = 3 });
    var p = (await s.ListAsync("TEST-01", 1)).Items.Single(); partId = p.Id; Assert(p.Stock == 0 && p.PriceCents == 12550);
});
await Check("Duplicate part and sub-cent price rejected", async () => {
    await using var db = Context(); var s = new PartService(db);
    await Reject(() => s.SaveAsync(new() { Code = "test-01", Name = "Duplicate", Price = 1 }));
    await Reject(() => s.SaveAsync(new() { Code = "NEW-01", Name = "Invalid", Price = 1.001m }));
});
await Check("Stock receive, stale version and negative quantity", async () => {
    await using var db = Context(); var s = new PartService(db); var p = (await s.GetAsync(partId))!;
    await s.ReceiveAsync(new() { Id = p.Id, Version = p.Version, Quantity = 10 });
    await Reject(() => s.ReceiveAsync(new() { Id = p.Id, Version = p.Version, Quantity = 5 }));
    await Reject(() => s.ReceiveAsync(new() { Id = p.Id, Version = p.Version, Quantity = -1 }));
    Assert((await s.GetAsync(p.Id))!.Stock == 10);
});
await Check("Service invalid date, mileage and labor rejected", async () => {
    await using var db = Context(); var s = new ServiceRecordService(db);
    await Reject(() => s.CreateAsync(new() { VehicleId = vehicleId, Date = DateTime.Today.AddDays(1), Mileage = 10000, Description = "Test servis" }));
    await Reject(() => s.CreateAsync(new() { VehicleId = vehicleId, Mileage = 9999, Description = "Test servis" }));
    await Reject(() => s.CreateAsync(new() { VehicleId = vehicleId, Mileage = 10000, Labor = -1, Description = "Test servis" }));
});
await Check("Create service updates mileage atomically", async () => {
    await using var db = Context(); serviceId = await new ServiceRecordService(db).CreateAsync(new() { VehicleId = vehicleId, Mileage = 11000, Description = "Test bakım işlemi", Labor = 500.25m });
    Assert((await db.Vehicles.FindAsync(vehicleId))!.Mileage == 11000);
});
await Check("Second open service and history deletion rejected", async () => {
    await using var db = Context(); await Reject(() => new ServiceRecordService(db).CreateAsync(new() { VehicleId = vehicleId, Mileage = 11000, Description = "İkinci servis" }));
    var v = (await db.Vehicles.FindAsync(vehicleId))!; await Reject(() => new VehicleService(db).DeleteAsync(v.Id, v.Version));
});
await Check("Insufficient stock leaves all rows unchanged", async () => {
    await using var db = Context(); var s = new ServiceRecordService(db); var r = (await s.GetAsync(serviceId))!;
    await Reject(() => s.AddPartAsync(new() { Id = r.Id, Version = r.Version, PartId = partId, Quantity = 11 }));
    Assert(await db.ServiceParts.CountAsync(x => x.ServiceRecordId == serviceId) == 0 && (await db.Parts.FindAsync(partId))!.Stock == 10);
});
await Check("Missing part selection is rejected without affecting stock or service", async () => {
    await using var db = Context(); var s = new ServiceRecordService(db); var r = (await s.GetAsync(serviceId))!;
    try { await s.AddPartAsync(new() { Id = r.Id, Version = r.Version, PartId = 0, Quantity = 2 }); }
    catch (RuleException error)
    {
        Assert(error.Message.Contains("Parça seçiniz.") && (await db.Parts.FindAsync(partId))!.Stock == 10);
        Assert(!await db.ServiceParts.AnyAsync(x => x.ServiceRecordId == serviceId)); return;
    }
    throw new Exception("Missing part selection was accepted");
});
await Check("Add part deducts stock and calculates total", async () => {
    await using var db = Context(); var s = new ServiceRecordService(db); var r = (await s.GetAsync(serviceId))!;
    await s.AddPartAsync(new() { Id = r.Id, Version = r.Version, PartId = partId, Quantity = 2 });
    Assert((await s.GetAsync(r.Id))!.TotalCents == 75125 && (await db.Parts.FindAsync(partId))!.Stock == 8);
});
await Check("Price change preserves historical line price", async () => {
    await using var db = Context(); var p = (await db.Parts.FindAsync(partId))!;
    await new PartService(db).SaveAsync(new() { Id = p.Id, Version = p.Version, Code = p.Code, Name = p.Name, Price = 999, MinimumStock = 3 });
    Assert((await new ServiceRecordService(db).GetAsync(serviceId))!.TotalCents == 75125);
});
await Check("Duplicate service part rejected without stock movement", async () => {
    await using var db = Context(); var s = new ServiceRecordService(db); var r = (await s.GetAsync(serviceId))!;
    await Reject(() => s.AddPartAsync(new() { Id = r.Id, Version = r.Version, PartId = partId, Quantity = 1 })); Assert((await db.Parts.FindAsync(partId))!.Stock == 8);
});
await Check("Cancellation returns stock exactly once", async () => {
    await using var db = Context(); var s = new ServiceRecordService(db); var r = (await s.GetAsync(serviceId))!;
    await s.ChangeStatusAsync(r.Id, r.Version, ServiceStatus.Cancelled);
    r = (await s.GetAsync(serviceId))!;
    await Reject(() => s.ChangeStatusAsync(r.Id, r.Version, ServiceStatus.Cancelled));
    await Reject(() => s.AddPartAsync(new() { Id = r.Id, Version = r.Version, PartId = partId, Quantity = 1 }));
    Assert((await db.Parts.FindAsync(partId))!.Stock == 10);
});
await Check("Used part cannot be deleted", async () => {
    await using var db = Context(); var s = new PartService(db); var p = (await s.GetAsync(partId))!; await Reject(() => s.DeleteAsync(p.Id, p.Version));
});
await Check("Completion locks service and feeds dashboard", async () => {
    await using var db = Context(); var s = new ServiceRecordService(db);
    var id = await s.CreateAsync(new() { VehicleId = vehicleId, Mileage = 12000, Description = "Tamamlanacak servis", Labor = 100 });
    var r = (await s.GetAsync(id))!; await s.ChangeStatusAsync(id, r.Version, ServiceStatus.Completed);
    r = (await s.GetAsync(id))!; await Reject(() => s.ChangeStatusAsync(id, r.Version, ServiceStatus.Cancelled));
    var dashboard = await new DashboardService(db).GetAsync(); Assert(dashboard.CompletedCents == 270000 && dashboard.OpenServices == 1 && dashboard.LowStock.Count == 1);
});
await Check("Invalid status rejected", async () => {
    await using var db = Context(); await Reject(() => new ServiceRecordService(db).ChangeStatusAsync(serviceId, Guid.NewGuid(), (ServiceStatus)99));
});
await Check("Pagination clamps invalid page numbers", async () => {
    await using var db = Context(); var s = new CustomerService(db);
    Assert((await s.ListAsync(null, -100)).Number == 1 && (await s.ListAsync(null, int.MaxValue)).Number == 1);
});
await Check("Two contexts cannot silently overwrite customer", async () => {
    await using var first = Context(); await using var second = Context();
    var a = (await first.Customers.FindAsync(customerId))!; var b = (await second.Customers.FindAsync(customerId))!;
    a.Name = "First writer"; await first.SaveChangesAsync(); b.Name = "Lost update";
    try { await second.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return; }
    throw new Exception("Lost update not prevented");
});
await Check("SQL CHECK prevents negative stock", async () => {
    await using var db = Context();
    try { await db.Database.ExecuteSqlRawAsync("UPDATE Parts SET Stock = -1"); } catch (SqliteException) { return; }
    throw new Exception("SQL constraint missing");
});
await Check("SQL FK prevents orphan vehicle", async () => {
    await using var db = Context();
    try { await db.Database.ExecuteSqlRawAsync("UPDATE Vehicles SET CustomerId = 999999"); } catch (SqliteException) { return; }
    throw new Exception("Foreign key missing");
});
await Check("Unlinked records can be deleted", async () => {
    await using var db = Context(); var s = new CustomerService(db);
    await s.SaveAsync(new() { Name = "Delete Me", Phone = "05320000000" }); var c = (await s.ListAsync("Delete Me", 1)).Items.Single();
    await s.DeleteAsync(c.Id, c.Version); Assert(await s.GetAsync(c.Id) == null);
});
await Check("Expanded demo catalog is repeatable and preserves existing stocks and prices", async () => {
    await using var db = Context();
    var part = await db.Parts.SingleAsync(x => x.Code == "FLT-002");
    part.Stock = 2; part.PriceCents = 12345; await db.SaveChangesAsync();
    var count = await db.Parts.CountAsync();
    await SeedData.InitializeAsync(db); await SeedData.InitializeAsync(db);
    await db.Entry(part).ReloadAsync();
    Assert(await db.Parts.CountAsync() == count && part.Stock == 2 && part.PriceCents == 12345);
});
Console.WriteLine($"SUCCESS: {passed} integration scenarios passed against relational SQLite.");
