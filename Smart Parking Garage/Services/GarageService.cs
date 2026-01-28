using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Contracts.Garage;

namespace Smart_Parking_Garage.Services;

public class GarageService:IGarageService
{

    private readonly ApplicationDbContext _context;

    public GarageService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GarageLocation>> GetAllGarageLocationsAsync(CancellationToken cancellationToken)
    {

        return await _context.Garages
            .Where(g => g.IsActive)
            .Select(g => new GarageLocation
            {
                GarageId = g.GarageId,
                Latitude = g.Latitude,
                Longitude = g.Longitude
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Garage>> GetAllAsync()
    {
        return await _context.Garages
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

}
