using Microsoft.AspNetCore.Mvc;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;

namespace NWSDB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
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

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // Get customer connected to logged-in User
        [HttpGet("by-user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var customer = await _service.GetByUserIdAsync(userId);

            if (customer == null)
            {
                return NotFound(new
                {
                    message = "Customer account was not found for this user."
                });
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CustomerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CustomerUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = await _service.UpdateAsync(id, dto);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}