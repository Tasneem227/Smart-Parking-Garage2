using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController(IFileService fileService) : ControllerBase
{
    private readonly IFileService _fileService = fileService;

    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var (fileContent, contentType, fileName) = await _fileService.DownloadAsync(id, cancellationToken);

        return fileContent is [] ? NotFound() : File(fileContent, contentType, fileName);
    }
}
