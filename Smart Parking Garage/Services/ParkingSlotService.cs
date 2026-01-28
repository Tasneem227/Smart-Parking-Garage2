
using Mapster;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;


namespace Smart_Parking_Garage.Services;

public class ParkingSlotService (ApplicationDbContext context): IParkingSlotService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<IEnumerable<ParkingSlot>> GetAllSlotsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ParkingSlots.ToListAsync(cancellationToken);
        
    }


    public async Task<ParkingSlot?> GetSlotByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ParkingSlots.FirstOrDefaultAsync(x=>x.ParkingSlotId == id , cancellationToken);
    }



    public async Task<IEnumerable<ParkingSlot>> GetAvailableSlotsAsync(CancellationToken cancellationToken = default)
    {
       return  await _context.ParkingSlots.Where(x => !x.IsOccupied).ToListAsync(cancellationToken);
      
    }


    public async Task<ParkingSlot> CreateSlotAsync(ParkingSlot request, CancellationToken cancellationToken = default)
    {
        _context.ParkingSlots.Add(request);
        await _context.SaveChangesAsync(cancellationToken);

        return request;
    }


    public async Task<bool> DeleteSlotAsync(int id, CancellationToken cancellationToken = default)
    {
        var slot = await GetSlotByIdAsync(id, cancellationToken);
        if (slot == null)
            return false;

        _context.Remove(slot);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }


    public async Task<bool> UpdateSlotAsync (int id, ParkingSlot request, CancellationToken cancellationToken = default)
    {
        var currentSlot = await GetSlotByIdAsync(id, cancellationToken);
        if (currentSlot == null)
            return false;

        currentSlot.SlotNumber = request.SlotNumber;
        currentSlot.SlotType = request.SlotType;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ToggleOccupancyAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentSlot = await GetSlotByIdAsync(id, cancellationToken);
        if (currentSlot == null)
            return false;

        currentSlot.IsOccupied = !currentSlot.IsOccupied;
        await _context.SaveChangesAsync(cancellationToken);

        return true;

    }
 


}
