using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Contracts.Payment;
using Smart_Parking_Garage.Entities;
using Smart_Parking_Garage.Errors;

namespace Smart_Parking_Garage.Services;
public class PaymentService(ApplicationDbContext context)
    : IPaymentService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PaymentResponse>> PayAsync(string userId,PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == request.BookingId,cancellationToken);

        if (booking is null)
            return Result.Failure<PaymentResponse>(PaymentErrors.BookingNotFound);

        if (booking.ApplicationUserId != userId)
            return Result.Failure<PaymentResponse>(PaymentErrors.BookingNotFound);

        bool alreadyPaid = await _context.Payments.AnyAsync(p => p.BookingId == booking.BookingId&& p.Status == "Success", cancellationToken);

        if (alreadyPaid)
            return Result.Failure<PaymentResponse>(PaymentErrors.AlreadyPaid);

        var card = await _context.MockCards.FirstOrDefaultAsync( c =>c.CardNumber == request.CardNumber && c.ExpiryDate == request.ExpiryDate && c.CVV == request.CVV,cancellationToken);

        if (card is null)
            return Result.Failure<PaymentResponse>( PaymentErrors.CardNotFound);

        if (card.IsBlocked)
            return Result.Failure<PaymentResponse>(PaymentErrors.CardBlocked);

        if (card.Balance < booking.Price)
            return Result.Failure<PaymentResponse>(PaymentErrors.InsufficientBalance);

        card.Balance -= booking.Price!.Value;

        var transactionId = Guid.NewGuid().ToString();

        var payment = new Payment
        {
            BookingId = booking.BookingId,
            MockCardId = card.MockCardId,
            Amount = booking.Price.Value,
            Status = "Success",
            PaymentMethod = "MockCard",
            TransactionId = transactionId,
            TransactionTime = DateTime.UtcNow,
            ApplicationUserId = userId
        };
        var garage = await _context.Garages.FirstOrDefaultAsync(g => g.GarageId == booking.GarageId);

        if (garage is not null)
        {
            garage.TotalRevenue += payment.Amount;
        }
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success( new PaymentResponse( "Payment completed successfully"));
    }

}