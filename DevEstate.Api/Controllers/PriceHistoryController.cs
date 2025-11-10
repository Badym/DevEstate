using DevEstate.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceHistoryController : ControllerBase
    {
        private readonly PriceHistoryService _service;

        public PriceHistoryController(PriceHistoryService service)
        {
            _service = service;
        }

        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetByPropertyId(string propertyId)
        {
            var history = await _service.GetByPropertyIdAsync(propertyId);
            return Ok(history);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var entry = await _service.GetByIdAsync(id);
            return Ok(entry);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var all = await _service.GetAllAsync();
            return Ok(all);
        }
    }
}