using DevEstate.Api.Services;
using DevEstate.Dtos.DeveloperOpenData;
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
    }
}
