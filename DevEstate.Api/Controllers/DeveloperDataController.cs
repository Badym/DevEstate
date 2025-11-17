using DevEstate.Services.DeveloperOpenData;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/developer-data")]
public class DeveloperDataController : ControllerBase
{
    private readonly DatasetFinder _finder;

    public DeveloperDataController(DatasetFinder finder)
    {
        _finder = finder;
    }

    [HttpGet("developer-datasets")]
    public async Task<IActionResult> DeveloperDatasets()
    {
        var data = await _finder.GetDeveloperDatasetsAsync();
        return Ok(data);
    }


}
