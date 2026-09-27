using Microsoft.AspNetCore.Mvc;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;

namespace NWSDB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsageController : ControllerBase
    {
        private readonly IUsageService _service;

        public UsageController(IUsageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetUsageRecordsAsync();
            return Ok(list);
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var list = await _service.GetByCustomerAsync(customerId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsageDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(null, created);
        }
    }
}
