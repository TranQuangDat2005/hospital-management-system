using FinancialBillingService.Interfaces;
using FinancialBillingService.Model;

namespace FinancialBillingService.Services
{
    public class ServiceCatalogService : IServiceCatalogService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceCatalogService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            return await _serviceRepository.GetAllAsync(); 
        }

        public async Task<IEnumerable<Service>> GetActiveServicesAsync()
        {
            return await _serviceRepository.GetAllActiveServicesAsync();
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            return await _serviceRepository.GetByIdAsync(id);
        }

        public async Task<Service> CreateServiceAsync(Service service)
        {
            if (service.Price <= 0)
            {
                throw new ArgumentException("Service price must be greater than 0");
            }
            if (string.IsNullOrWhiteSpace(service.Category))
            {
                throw new ArgumentException("Service must belong to a specific category");
            }
            
            return await _serviceRepository.CreateAsync(service);
        }

        public async Task<Service> UpdateServiceAsync(int id, Service serviceUpdate)
        {
            var existingService = await _serviceRepository.GetByIdAsync(id);
            if (existingService == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }

            if (serviceUpdate.Price <= 0)
            {
                throw new ArgumentException("Service price must be greater than 0");
            }

            existingService.ServiceName = serviceUpdate.ServiceName;
            existingService.Category = serviceUpdate.Category;
            existingService.Price = serviceUpdate.Price;
            existingService.Status = serviceUpdate.Status;

            return await _serviceRepository.UpdateAsync(existingService);
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
             return await _serviceRepository.DeleteAsync(id);
        }
    }
}
