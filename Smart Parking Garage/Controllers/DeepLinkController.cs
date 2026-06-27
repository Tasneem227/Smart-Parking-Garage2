using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Smart_Parking_Garage.Controllers;
[ApiController]
[Route("auth")]
public class DeepLinkController : Controller
{
    [HttpGet("open-email-confirmed")]
    public IActionResult OpenEmailConfirmed(
    string status,
    string? reason = null)
    {
        var appUrl =
            $"https://smartparking://auth/email-confirmed?status={status}";

        if (!string.IsNullOrWhiteSpace(reason))
        {
            appUrl += $"&reason={Uri.EscapeDataString(reason)}";
        }

        var html = $"""
                        <!doctype html>
                        <html>
                        <head>
                        <meta http-equiv="refresh" content="0; url={appUrl}">
                        <title>Open Smart Parking</title>
                        </head>
                        <body>
                        <p>Opening Smart Parking...</p>

                        <p>
                        If nothing happens,
                        <a href="{appUrl}">
                        Click here
                        </a>
                        </p>
                        </body>
                        </html>
                        """;

        return Content(html, "text/html");
    }
}
