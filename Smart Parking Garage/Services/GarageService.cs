using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Abstractions.Consts;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;
using Smart_Parking_Garage.Contracts.Garage;
using Smart_Parking_Garage.Errors;
using System.Data;

namespace Smart_Parking_Garage.Services;

public class GarageService:IGarageService
{

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _UserManager;

    public GarageService(ApplicationDbContext context
                        , UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _UserManager = userManager;
    }

    public async Task<List<GarageLocation>> GetAllGarageLocationsAsync(CancellationToken cancellationToken)
    {

        return await _context.Garages
       .Where(g => g.IsActive)
       .Select(g => new GarageLocation
       {
           GarageId = g.GarageId,
           GarageName=g.Name,
           Latitude = g.Latitude,
           Longitude = g.Longitude,
           Slots = g.ParkingSlots.Select(s => new GarageSlots
           {
               SlotId = s.ParkingSlotId,
               Slotnumber= s.SlotNumber,
               Status = s.IsOccupied ? "Occupied" : "Available"
           }).ToList()
       })
       .ToListAsync(cancellationToken);
    }
    public async Task<GarageSlotsStatus?> GetSlotsStatusByGarageIdAsync(
    int garageId,
    CancellationToken cancellationToken)
    {
        return await _context.Garages
            .Where(g => g.GarageId == garageId && g.IsActive)
            .Select(g => new GarageSlotsStatus
            {
                GarageId = g.GarageId,
                TotalSlots = g.ParkingSlots.Count(),
                AvailableSlots = g.ParkingSlots.Count(s => !s.IsOccupied),
                Slots = g.ParkingSlots.Select(s => new SlotStatus
                {
                    SlotId = s.ParkingSlotId,
                    Slotnumber = s.SlotNumber,
                    SalaryPerHour=s.PricePerHour,
                    Status = s.IsOccupied ? "Occupied" : "Available"
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<List<Garage>> GetAllAsync()
    {
        return await _context.Garages
            .Select(g => new Garage
            {
                GarageId = g.GarageId,
                OwnerId=g.OwnerId,
                Name = g.Name,
                Address = g.Address,
                Latitude = g.Latitude,
                Longitude = g.Longitude,
                TotalSlots = g.TotalSlots,
                AvailableSlots = g.AvailableSlots,
                IsActive = g.IsActive
            }).ToListAsync();
    }

    public async Task<Garage?> GetByIdAsync(int id)
    {
        return await _context.Garages
            .Where(g => g.GarageId == id)
            .Select(g => new Garage     
            {
                GarageId = g.GarageId,
                Name = g.Name,
                Address = g.Address,
                Latitude = g.Latitude,
                Longitude = g.Longitude,
                TotalSlots = g.TotalSlots,
                AvailableSlots = g.AvailableSlots,
                IsActive = g.IsActive
            }).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(GarageCreate dto)
    {
        var garage = new Garage
        {
            Name = dto.Name,
            Address = dto.Address,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            TotalSlots = dto.TotalSlots,
            AvailableSlots = dto.TotalSlots,
            IsActive = true
        };

        _context.Garages.Add(garage);
        await _context.SaveChangesAsync();

        
    }

    public async Task<bool> UpdateAsync(int id, GarageCreate  dto)
    {
        var garage = await _context.Garages.FindAsync(id);
        if (garage == null) return false;

        garage.Name = dto.Name;
        garage.Address = dto.Address;
        garage.Latitude = dto.Latitude;
        garage.Longitude = dto.Longitude;
        garage.TotalSlots = dto.TotalSlots;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var garage = await _context.Garages.FindAsync(id);
        if (garage == null) return false;

        _context.Garages.Remove(garage);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<Result<GarageOwnerGatesAndGaragesRequest>> GarageOwnerGaragesAndGates(
     string garageOwnerId,CancellationToken cancellationToken)
    {
        var user = await _UserManager.FindByIdAsync(garageOwnerId.ToString());

        if (user is null)
            return Result.Failure<GarageOwnerGatesAndGaragesRequest>(
                UserErrors.UserNotFound);

        var isGarageOwner = await _UserManager.IsInRoleAsync(
            user,
            DefaultRoles.GarageOwner);

        if (!isGarageOwner)
            return Result.Failure<GarageOwnerGatesAndGaragesRequest>(
                UserErrors.NotGarageOwner);

        var garages = await _context.Garages
            .Where(g => g.OwnerId == garageOwnerId)
            .Select(g => new GarageOwnerGatesAndGaragesRequest(
                g.GarageId,
                g.Gates.Select(gt => gt.GateId)
            ))
            .SingleOrDefaultAsync(cancellationToken);

        return Result.Success<GarageOwnerGatesAndGaragesRequest>(garages);
    }

    public async Task<Result<decimal>> GetGarageRevenueAsync(
    int garageId,
    CancellationToken cancellationToken = default)
    {
        var garageExists = await _context.Garages
            .AnyAsync(g => g.GarageId == garageId, cancellationToken);

        if (!garageExists)
            return Result.Failure<decimal>(GarageErrors.GarageNotFound);

        var totalRevenue = await _context.Bookings
            .Where(b => b.GarageId == garageId &&
                        b.Status == BookingStatuses.Completed)
            .SumAsync(b => b.Price ?? 0, cancellationToken);

        return Result.Success(totalRevenue);
    }
}
