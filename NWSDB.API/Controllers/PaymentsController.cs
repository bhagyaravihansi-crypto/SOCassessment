using Microsoft.AspNetCore.Mvc;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;

namespace NWSDB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        // GET: api/Payments/customer/1
        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var list = await _service.GetByCustomerAsync(customerId);
            return Ok(list);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] PaymentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetAll),
                created);
        }
    }
}