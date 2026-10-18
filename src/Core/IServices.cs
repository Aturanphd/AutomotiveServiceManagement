namespace Core;
public interface ICustomerService
{
    Task<Page<Customer>> ListAsync(string? q, int page);
    Task<Customer?> GetAsync(int id);
    Task SaveAsync(CustomerForm form);
    Task DeleteAsync(int id, Guid version);
}
public interface IVehicleService
{
    Task<Page<Vehicle>> ListAsync(string? q, int page);
    Task<Vehicle?> GetAsync(int id);
    Task SaveAsync(VehicleForm form);
    Task DeleteAsync(int id, Guid version);
    Task<List<Customer>> CustomersAsync();
}
public interface IPartService
{
    Task<Page<Part>> ListAsync(string? q, int page);
    Task<Part?> GetAsync(int id);
    Task SaveAsync(PartForm form);
    Task DeleteAsync(int id, Guid version);
    Task ReceiveAsync(QuantityForm form);
}
public interface IServiceRecordService
{
    Task<Page<ServiceRecord>> ListAsync(string? q, int page);
    Task<ServiceRecord?> GetAsync(int id);
    Task<int> CreateAsync(ServiceForm form);
    Task AddPartAsync(AddServicePartForm form);
    Task ChangeStatusAsync(int id, Guid version, ServiceStatus status);
    Task<List<Vehicle>> VehiclesAsync();
    Task<List<Part>> PartsAsync();
}
public interface IDashboardService { Task<Dashboard> GetAsync(); }
