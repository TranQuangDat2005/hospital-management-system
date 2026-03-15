using FinancialBillingService.DTOs;
using FinancialBillingService.Interfaces;
using FinancialBillingService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBillingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ServiceCatalogController : ControllerBase
    {
        private readonly IServiceCatalogService _serviceCatalogService;

        public ServiceCatalogController(IServiceCatalogService serviceCatalogService)
        {
            _serviceCatalogService = serviceCatalogService;
        }
        [HttpGet("active")]
        [AllowAnonymous]
        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveServices()
        {
            var services = await _serviceCatalogService.GetActiveServicesAsync();
            return Ok(services);
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicServices()
        {
            var services = await _serviceCatalogService.GetActiveServicesAsync();
            return Ok(services);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceCatalogService.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _serviceCatalogService.GetServiceByIdAsync(id);
            if (service == null) return NotFound(new { message = "Service not found" });
            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceCreationDto dto)
        {
            try
            {
                var newService = new Service
                {
                    ServiceName = dto.ServiceName,
                    Category = dto.Category,
                    Price = dto.Price,
                    Status = dto.Status
                };

                var createdService = await _serviceCatalogService.CreateServiceAsync(newService);
                return CreatedAtAction(nameof(GetServiceById), new { id = createdService.ServiceID }, createdService);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] ServiceCreationDto dto)
        {
            try
            {
                var updateService = new Service
                {
                    ServiceID = id,
                    ServiceName = dto.ServiceName,
                    Category = dto.Category,
                    Price = dto.Price,
                    Status = dto.Status
                };

                var updatedService = await _serviceCatalogService.UpdateServiceAsync(id, updateService);
                return Ok(updatedService);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await _serviceCatalogService.DeleteServiceAsync(id);
            if (!result) return NotFound(new { message = "Service not found or could not be deleted" });
            return NoContent();
        }
    }
}
