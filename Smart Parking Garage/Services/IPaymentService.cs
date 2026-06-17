using Smart_Parking_Garage.Contracts.Payment;

namespace Smart_Parking_Garage.Services;

public interface IPaymentService
{
        Task<Result<PaymentResponse>> PayAsync( string userId,PaymentRequest request,CancellationToken cancellationToken = default);   
}

