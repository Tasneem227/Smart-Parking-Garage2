using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Contracts.Payment;
using Smart_Parking_Garage.Services;
using System.Security.Claims;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PaymentsController(IPaymentService paymentService) : ControllerBase
{
    private readonly IPaymentService _paymentService = paymentService;
    
    [HttpPost]
    public async Task<IActionResult> Pay( PaymentRequest request,CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _paymentService.PayAsync(userId,request,cancellationToken);
        return result.IsSuccess? Ok(result.Value): result.ToProblem();
    }
}
