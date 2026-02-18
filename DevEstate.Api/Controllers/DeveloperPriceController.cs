using DevEstate.Api.Services;
using DevEstate.Dtos.DeveloperOpenData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeveloperPriceController : ControllerBase
    {
        private readonly DeveloperPriceService _service;

        public DeveloperPriceController(DeveloperPriceService service)
        {
            _service = service;
        }

        // --------------------------------------------------------
        // 1) DODAWANIE pełnego rekordu
        // --------------------------------------------------------
        [HttpPost("add")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> AddPrice([FromBody] DeveloperPriceRecordAggregatedDto dto)
        {
            if (dto == null || dto.CenaZaM2 == null)
                return BadRequest(new { message = "Invalid data — CenaZaM2 is required." });

            try
            {
                await _service.AddRecordAsync(dto);
                return Ok(new { message = "Record successfully created." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error while inserting record.",
                    details = ex.Message
                });
            }
        }

        // --------------------------------------------------------
        // 2) AKTUALIZACJA istniejącego regionu
        // --------------------------------------------------------
        [HttpPut("update")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> UpdatePrice([FromBody] DeveloperPriceRecordUpdateDto dto)
        {
            if (dto == null || dto.CenaZaM2 == null)
                return BadRequest(new { message = "Invalid data — CenaZaM2 is required." });

            try
            {
                await _service.UpdateAsync(dto);
                return Ok(new { message = "Record successfully updated." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error while updating record.",
                    details = ex.Message
                });
            }
        }
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string? woj, [FromQuery] string? pow)
        {
            var data = await _service.GetFilteredAsync(woj, pow);
            return Ok(data);
        }
        
        // GET: api/developerprice/wojewodztwa
        [HttpGet("wojewodztwa")]
        public async Task<IActionResult> GetWojewodztwa()
        {
            var list = await _service.GetDistinctWojewodztwaAsync();
            return Ok(list);
        }

// GET: api/developerprice/powiaty?woj=slaskie
        [HttpGet("powiaty")]
        public async Task<IActionResult> GetPowiaty([FromQuery] string? woj)
        {
            var list = await _service.GetDistinctPowiatyAsync(woj);
            return Ok(list);
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string woj, [FromQuery] string? pow)
        {
            var data = await _service.GetRegionAggregatedAsync(woj, pow); //zmiana

            if (data == null)
                return NotFound(new { message = "Region not found" });

            return Ok(data);
        }



    }
}
