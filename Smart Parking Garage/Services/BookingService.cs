
using Mapster;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Entities;

using System.Security.Claims;

namespace Smart_Parking_Garage.Services;

public class BookingService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, INotificationService notificationService) : IBookingService
{
    private readonly ApplicationDbContext _Context = context;
    private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;
    private readonly INotificationService _notificationService = notificationService;

    //Add Booking
    public async Task<BookingResponse> AddBooking(BookingRequest request, CancellationToken cancellationToken = default)
    {
        //handle to allow booking if ater the slot book time
        var slot = _Context.ParkingSlots?.FirstOrDefault(b => b.SlotNumber == request.SlotNumber && b.GarageId == request.GarageId);
        var isOccupied = slot?.IsOccupied;
        if (!(bool)isOccupied)
        {
            Booking booking = request.Adapt<Booking>();
            booking.ParkingSlotId = slot.ParkingSlotId;
            if (request.ApplicationUserId == null)
            {
                booking.ApplicationUserId = _HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            booking.Status = "Active";
            if (booking != null)
            {
                await _Context.AddAsync(booking, cancellationToken);
                slot.IsOccupied = true;
                await _Context.SaveChangesAsync();
                ////Garage Full
                //var garage = await _Context.Garages .Include(g => g.ParkingSlots).FirstOrDefaultAsync(g => g.GarageId == request.GarageId);

                //if (garage != null)
                //{
                //    garage.AvailableSlots = garage.ParkingSlots.Count(s => !s.IsOccupied);

                //    if (garage.AvailableSlots == 0)
                //    {
                //        await _notificationService.SendAsync(
                //            booking.ApplicationUserId,
                //            "Garage Full ⚠️",
                //            $"Garage {garage.Name} is now full",
                //            "System"
                //        );
                //    }
                //}

                //Booking Confirmed
                await _notificationService.SendAsync(
                   booking.ApplicationUserId,
                   "Booking Confirmed 🚗",
                 $"Your slot {slot.SlotNumber} at garage {slot.GarageId} has been booked successfully",
                    "Booking"
                );
                return booking.Adapt<BookingResponse>();
            }
            throw new Exception("the booking data are invalid");
        }
        throw new Exception("the Parking slot is occupied");

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
}
