
using Mapster;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Abstractions.Consts;
using Smart_Parking_Garage.Entities;
using Smart_Parking_Garage.Errors;
using System.Security.Claims;

namespace Smart_Parking_Garage.Services;

public class BookingService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, INotificationService notificationService) : IBookingService
{
    private readonly ApplicationDbContext _Context = context;
    private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;
    private readonly INotificationService _notificationService = notificationService;

    //Add Booking
    public async Task<Result<BookingResponse>> AddBooking(BookingRequest request, string userid,CancellationToken cancellationToken = default)
    {
        //handle to allow booking if ater the slot book time
        var ExistUserCarType = await _Context.CarTypes.AnyAsync(x => x.carType == request.CarType.ToLower()
                                                        && x.UserId == userid,
                                                          cancellationToken);
        if (!ExistUserCarType) {
            return Result.Failure<BookingResponse>(CarTypeErrors.UserCarTypeNotFound);
        }


        var slot =await _Context.ParkingSlots.Include(b=>b.Bookings).FirstOrDefaultAsync(
                                                                     b =>b.GarageId == request.GarageId
                                                                     && b.SlotType==request.CarType
                                                                     && (
                                                                        !b.IsOccupied ||
                                                                        !b.Bookings.Any(b =>
                                                                            (b.Status == BookingStatuses.Active ||
                                                                             b.Status == BookingStatuses.Pending) &&
                                                                            b.BookingEnd >= request.BookingStart
                                                                        ))
                                                                     , cancellationToken);
        
        if (slot is null) {
            return Result.Failure<BookingResponse>(ParkingSlotErrors.NoEmptySlotForCarType);
        }
        
        Booking booking = request.Adapt<Booking>();
        booking.ParkingSlotId = slot.ParkingSlotId;
        booking.ApplicationUserId=userid;
        var minutes = (decimal)(request.BookingEnd - request.BookingStart).TotalMinutes;
        var totalPrice = (minutes / 60m) * slot.PricePerHour;
        booking.Price = Math.Round(totalPrice, 2);

        var now = DateTime.UtcNow;
        booking.Status = booking.BookingStart > now
            ? BookingStatuses.Pending
            : BookingStatuses.Active;

        await _Context.AddAsync(booking, cancellationToken);
        slot.IsOccupied = true;
        await _Context.SaveChangesAsync();
             
        //Booking Confirmed
        await _notificationService.SendAsync(
            booking.ApplicationUserId,
            "Booking Confirmed 🚗",
            $"Your slot {slot.SlotNumber} at garage {slot.GarageId} has been booked successfully",
            "Booking"
        );
        var bookingResponse = booking.Adapt<BookingResponse>();
        //dbookingResponse.SlotNumber=slot.SlotNumber;

        return Result.Success(bookingResponse);
          
    }

    public async Task<IEnumerable<BookingResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _Context.Bookings
            .Include(s => s.ParkingSlot)
            .AsNoTracking()
            .ProjectToType<BookingResponse>()
            .ToListAsync(cancellationToken);
    }

    public async Task<BookingResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var Booking = await _Context.Bookings.FindAsync(id, cancellationToken);
        if (Booking != null)
        {
            return Booking.Adapt<BookingResponse>();
        }
        throw new Exception("there is no booking with this Booking Id");
    }
    public async Task<List<BookingResponse>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var Booking = await _Context.Bookings.Where(x => x.ApplicationUserId == userId).ToListAsync(cancellationToken);
        if (Booking != null)
        {
            return Booking.Adapt<List<BookingResponse>>();
        }
        throw new Exception("there is no booking For this Id");
    }
    //    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    //    {
    //        var Booking = await _Context.Bookings.FindAsync(id, cancellationToken);
    //        if (Booking != null)
    //        {
    //            var parkingslot = _Context.ParkingSlots.FirstOrDefault(i => i.ParkingSlotId == Booking.ParkingSlotId);
    //            if (parkingslot != null)
    //            {
    //                parkingslot.IsOccupied = false;
    //                _Context.Bookings.Remove(Booking);
    //                await _Context.SaveChangesAsync();
    //                await _notificationService.SendAsync(
    //                               Booking.ApplicationUserId,
    //                         "Booking Cancelled ❌",
    //                       "Your booking has been cancelled",
    //                          "Booking"
    //);
    //                return;
    //            }
    //        }
    //            throw new Exception("there is no booking with this Booking Id");

    //    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var Booking = await _Context.Bookings.FindAsync(id, cancellationToken);
        if (Booking != null)
        {
            var parkingslot = _Context.ParkingSlots.FirstOrDefault(i => i.ParkingSlotId == Booking.ParkingSlotId);
            if (parkingslot != null)
            {
                parkingslot.IsOccupied = false;


                var garage = await _Context.Garages
                    .Include(g => g.ParkingSlots)
                    .FirstOrDefaultAsync(g => g.GarageId == Booking.GarageId, cancellationToken);

                if (garage != null)
                {
                    garage.AvailableSlots = garage.ParkingSlots.Count(s => !s.IsOccupied);
                }

                _Context.Bookings.Remove(Booking);
                await _Context.SaveChangesAsync();

                await _notificationService.SendAsync(
                    Booking.ApplicationUserId,
                    "Booking Cancelled ❌",
                    "Your booking has been cancelled",
                    "Booking"
                );

                return;
            }
        }
        throw new Exception("there is no booking with this Booking Id");
    }

    public async Task<BookingResponse> UpdateBookingTimeAsync(int id, updateBookingTimeRequest request, CancellationToken cancellationToken)
    {
        var booking = await _Context.Bookings.FindAsync(new object[] { id }, cancellationToken);
        if (booking == null) return null;

        booking.BookingStart = request.BookingStart;
        booking.BookingEnd = request.BookingEnd;

        await _Context.SaveChangesAsync(cancellationToken);

        return booking.Adapt<BookingResponse>();
    }
    public async Task<bool> UpdateBookingStatusAsync(int id, UpdateBookingStatusRequest request, CancellationToken cancellationToken)
    {
        var booking = await _Context.Bookings.FindAsync(new object[] { id }, cancellationToken);
        if (booking == null) return false;

        booking.Status = request.status;

        await _Context.SaveChangesAsync(cancellationToken);

        return true;
    }

    //    public async Task DeleteByLastBookingByUserId(string userid, CancellationToken cancellationToken = default)
    //    {
    //        var booking = await _Context.Bookings
    //       .Where(b => b.ApplicationUserId == userid)
    //       .OrderByDescending(b => b.BookingStart)
    //       .ThenByDescending(b => b.BookingId)
    //       .FirstOrDefaultAsync(cancellationToken);

    //        if (booking == null)
    //            throw new Exception("No booking found for this user");

    //        var parkingSlot = await _Context.ParkingSlots
    //            .FirstOrDefaultAsync(p => p.ParkingSlotId == booking.ParkingSlotId, cancellationToken);

    //        if (parkingSlot != null)
    //            parkingSlot.IsOccupied = false;

    //        _Context.Bookings.Remove(booking);
    //        await _Context.SaveChangesAsync();
    //        await _notificationService.SendAsync(
    //                              booking.ApplicationUserId,
    //                        "Last Booking Cancelled ❌",
    //                      "Your Last booking has been cancelled",
    //                         "Booking"
    //);


    //   }

    public async Task DeleteByLastBookingByUserId(string userid, CancellationToken cancellationToken = default)
    {
        var booking = await _Context.Bookings
            .Where(b => b.ApplicationUserId == userid)
            .OrderByDescending(b => b.BookingStart)
            .ThenByDescending(b => b.BookingId)
            .FirstOrDefaultAsync(cancellationToken);

        if (booking == null)
            throw new Exception("No booking found for this user");

        var parkingSlot = await _Context.ParkingSlots
            .FirstOrDefaultAsync(p => p.ParkingSlotId == booking.ParkingSlotId, cancellationToken);

        if (parkingSlot != null)
            parkingSlot.IsOccupied = false;


        var garage = await _Context.Garages
            .Include(g => g.ParkingSlots)
            .FirstOrDefaultAsync(g => g.GarageId == booking.GarageId, cancellationToken);

        if (garage != null)
        {
            garage.AvailableSlots = garage.ParkingSlots.Count(s => !s.IsOccupied);
        }

        _Context.Bookings.Remove(booking);
        await _Context.SaveChangesAsync();

        await _notificationService.SendAsync(
            booking.ApplicationUserId,
            "Last Booking Cancelled ❌",
            "Your Last booking has been cancelled",
            "Booking"
        );
    }

    public async Task<Booking?> GetCurrentBookingForGateAsync(string userId,CancellationToken cancellationToken = default)
    {
        return await _Context.Bookings
       .Where(b =>b.ApplicationUserId == userId
           && b.Status != "Cancelled"
           && b.Status != "Completed"
           && b.Status != "Expired")
       .OrderBy(b => b.BookingStart)
       .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Booking?> GetCurrentBookingForExitGateAsync(string userId,CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await _Context.Bookings
            .Where(b => b.ApplicationUserId == userId
                && b.Status != "Completed"
                && b.Status != "Cancelled"
                && b.Status != "Expired"
                && b.BookingStart <= now
                && b.BookingEnd.HasValue
                && b.BookingEnd >= now)
            .OrderByDescending(b => b.BookingStart)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
