using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Contracts.uploadedFile;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AIModelsController(IAIModelsService aIModelsService) : ControllerBase
{
    private readonly IAIModelsService _AIModelsService = aIModelsService;

    [HttpPost("classify")]
    public async Task<IActionResult> Classify(
      [FromForm]  UploadedImageRequest image)
    {
        
        var result =
            await _AIModelsService.ClassifyVehicleAsync(image, User.GetUserId()!);

        return Ok(result.Value);
    }
    [HttpPost("SlotsAnalysis")]
    public async Task<IActionResult> Analysis(
      [FromForm] UploadedGarageImageRequest photo)
    {

        var result =
            await _AIModelsService.AnalyzeParkingImageAsync(photo);

        return Ok(result);
    }
}
