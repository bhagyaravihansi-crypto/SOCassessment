using Microsoft.AspNetCore.Mvc;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;

namespace NWSDB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _service;

        public BillsController(IBillService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var list = await _service.GetByCustomerAsync(customerId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BillCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
