
using Azure.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Contracts.ParkingSlot;
using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;


namespace Smart_Parking_Garage.Services;

public class ParkingSlotService (ApplicationDbContext context): IParkingSlotService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<IEnumerable<ParkingSlotResponse>>> GetAllSlotsAsync(CancellationToken cancellationToken = default)
    {
        var AllSlots = await _context.ParkingSlots.ToListAsync(cancellationToken);
        return Result.Success(AllSlots.Adapt<IEnumerable<ParkingSlotResponse>>());
    }

    public async Task<Result<ParkingSlotResponse>> GetSlotByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var slot = await _context.ParkingSlots.FirstOrDefaultAsync(x => x.ParkingSlotId == id, cancellationToken);
        if (slot is null) 
            return Result.Failure<ParkingSlotResponse>(ParkingSlotErrors.SlotNotFound);

        return Result.Success(slot.Adapt<ParkingSlotResponse>());
    }

    public async Task<Result<ParkingSlotResponse>> CreateSlotAsync(ParkingSlotRequest request, CancellationToken cancellationToken = default)
    {
        var garageExists = await _context.Garages.AnyAsync(g => g.GarageId == request.GarageId,cancellationToken);

        if (!garageExists)
            Result.Failure<ParkingSlotResponse>(ParkingSlotErrors.GarageNotFound);

        var newSlot = request.Adapt<ParkingSlot>();
        _context.ParkingSlots.Add(newSlot);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(newSlot.Adapt<ParkingSlotResponse>());
    }


    public async Task<Result<IEnumerable<ParkingSlotResponse>>> GetAvailableSlotsAsync(CancellationToken cancellationToken = default)
    {
        var AvailableSlots = await _context.ParkingSlots.Where(x => !x.IsOccupied).ToListAsync(cancellationToken);
        return Result.Success(AvailableSlots.Adapt<IEnumerable<ParkingSlotResponse>>());
    }
    //public async Task<Result> DeleteSlotAsync(int id, CancellationToken cancellationToken = default)
    //{
    //    var slot = await _context.ParkingSlots.FirstOrDefaultAsync(x => x.ParkingSlotId == id, cancellationToken);
    //    if (slot is null)
    //        return Result.Failure(ParkingSlotErrors.SlotNotFound);

    //    _context.Remove(slot);
    //    await _context.SaveChangesAsync(cancellationToken);
    //    return Result.Success();
    //}


    public async Task<Result> UpdateSlotAsync (int id, UpdateParkingSlotRequest request, CancellationToken cancellationToken = default)
    {
        var garageExists = await _context.Garages.AnyAsync(g => g.GarageId == request.GarageId, cancellationToken);
        if (!garageExists)
            return Result.Failure(ParkingSlotErrors.GarageNotFound);

        var currentSlot = await _context.ParkingSlots.FirstOrDefaultAsync(x => x.ParkingSlotId == id, cancellationToken);
        if (currentSlot is null)
            return Result.Failure(ParkingSlotErrors.SlotNotFound);

       

        currentSlot.SlotNumber = request.SlotNumber;
        currentSlot.SlotType = request.SlotType;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> ToggleOccupancyAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentSlot = await _context.ParkingSlots.FirstOrDefaultAsync(x => x.ParkingSlotId == id, cancellationToken);
        if (currentSlot is null)
            return Result.Failure(ParkingSlotErrors.SlotNotFound);

        currentSlot.IsOccupied = !currentSlot.IsOccupied;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<ParkingSlotResponse>>> GetSlotsByGarageIdAsync( int garageId,CancellationToken cancellationToken = default)
    {
        var garageExists = await _context.Garages.AnyAsync(g => g.GarageId == garageId, cancellationToken);

        if (!garageExists)
            Result.Failure<IEnumerable<ParkingSlotResponse>>(ParkingSlotErrors.GarageNotFound);

        var slots = await _context.ParkingSlots.Where(s => s.GarageId == garageId).ToListAsync(cancellationToken);
        if (slots is null)
            Result.Failure<IEnumerable<ParkingSlotResponse>>(ParkingSlotErrors.SlotsNotFound);

        return Result.Success(slots.Adapt<IEnumerable<ParkingSlotResponse>>());
    }


    public async Task<Result<IEnumerable<ParkingSlotResponse>>> GetAvailableSlotsByGarageIdAsync(int garageId,CancellationToken cancellationToken = default)
    {
        var garageExists = await _context.Garages.AnyAsync(g => g.GarageId == garageId, cancellationToken);

        if (!garageExists)
            Result.Failure<IEnumerable<ParkingSlotResponse>>(ParkingSlotErrors.GarageNotFound);

        var slots = await _context.ParkingSlots.Where(s => s.GarageId == garageId && !s.IsOccupied).ToListAsync(cancellationToken);
        if (slots is null)
            Result.Failure<IEnumerable<ParkingSlotResponse>>(ParkingSlotErrors.AvailableSlotsNotFound);

        return Result.Success(slots.Adapt<IEnumerable<ParkingSlotResponse>>());
    }
}
