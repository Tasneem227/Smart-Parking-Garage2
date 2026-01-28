
using Mapster;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Entities;
using Smart_Parking_Garage.Persistance;
using System.Security.Claims;

namespace Smart_Parking_Garage.Services;

public class BookingService(ApplicationDbContext context,IHttpContextAccessor httpContextAccessor):IBookingService
{
    private readonly ApplicationDbContext _Context = context;
    private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;

    //Add Booking
    public async Task<BookingResponse> AddBooking (BookingRequest request,CancellationToken cancellationToken=default)
    {
        //handle to allow booking if ater the slot book time
        var slot= _Context.ParkingSlots?.FirstOrDefault(b=>b.SlotNumber==request.SlotNumber);
        var isOccupied = slot?.IsOccupied;
        if (!(bool)isOccupied)
        {
            Booking booking = request.Adapt<Booking>();
            booking.ParkingSlotId = slot.ParkingSlotId;
            booking.ApplicationUserId = _HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier); ;
            booking.Status = "Active";
            if (booking != null) { 
                await  _Context.AddAsync(booking,cancellationToken);
                slot.IsOccupied = true;
                await _Context.SaveChangesAsync();
                return booking.Adapt<BookingResponse>();
            }
            throw new Exception("the booking data are invalid");
        }
        throw new Exception("the Parking slot is occupied");

    }

    public async Task<IEnumerable<BookingResponse>> GetAllAsync( CancellationToken cancellationToken = default)
    {
        return await _Context.Bookings
            .Include(s=>s.ParkingSlot)
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
        var Booking = await _Context.Bookings.Where(x=>x.ApplicationUserId == userId).ToListAsync(cancellationToken);
        if (Booking != null)
        {
            return Booking.Adapt<List<BookingResponse>>();
        }
        throw new Exception("there is no booking For this Id");
    }
    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var Booking = await _Context.Bookings.FindAsync(id, cancellationToken);
        if (Booking != null)
        {
            var parkingslot = _Context.ParkingSlots.FirstOrDefault(i => i.ParkingSlotId == Booking.ParkingSlotId);
            if (parkingslot != null)
            {
                parkingslot.IsOccupied = false;
                _Context.Bookings.Remove(Booking);
                await _Context.SaveChangesAsync();
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
}
