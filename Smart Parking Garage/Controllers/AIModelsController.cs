using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Contracts.uploadedFile;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AIModelsController([FromForm]IAIModelsService aIModelsService) : ControllerBase
{
    private readonly IAIModelsService _AIModelsService = aIModelsService;

    [HttpPost("classify")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Classify(
        UploadedImageRequest image)
    {
        
        var result =
            await _AIModelsService.ClassifyVehicleAsync(image, User.GetUserId()!);

        return Ok(result.Value);
    }
}
