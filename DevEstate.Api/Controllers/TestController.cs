using DevEstate.Api.Models;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Mvc;
using DevEstate.Api.Dtos;

namespace DevEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TestService _testService;

        public TestController(TestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tests = await _testService.GetAllAsync();
            return Ok(tests);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestCreateDto dto)
        {
            var test = new Test
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _testService.CreateAsync(test);
            return CreatedAtAction(nameof(GetAll), new { id = test.Id }, test);
        }
    }
}