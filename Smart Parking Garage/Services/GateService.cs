using Smart_Parking_Garage.Contracts.Gate;

namespace Smart_Parking_Garage.Services;

public class GateService(ApplicationDbContext context) : IGateService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<GateResponse>>> GetAllGatesAsync(CancellationToken cancellationToken = default)
    {
       var AllGates = await _context.Gates.ToListAsync(cancellationToken);
        return Result.Success(AllGates.Adapt<IEnumerable <GateResponse>> ());
    }


    public async Task<Result<GateResponse>> GetGateByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var gate = await _context.Gates.FirstOrDefaultAsync(x => x.GateId == id, cancellationToken);
        return gate is not null ? Result.Success(gate.Adapt<GateResponse>()) : Result.Failure<GateResponse>(GarageErrors.GarageNotFound) ;
    }

    public async Task<Result<GateResponse>> CreateGateAsync(GateRequest gate, CancellationToken cancellationToken = default)
    {
        var garageExists = await _context.Garages.AnyAsync(x => x.GarageId == gate.GarageId,cancellationToken);

        if (!garageExists)
           return Result.Failure<GateResponse>(GateErrors.GarageNotFound);

        var newGate = gate.Adapt<Gate>();
        _context.Gates.Add(newGate);

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(newGate.Adapt<GateResponse>());
    }


    public async Task<Result> UpdateGateAsync(int id, UpdateGateRequest gate, CancellationToken cancellationToken = default)
    {
        var currentGate = await _context.Gates.FirstOrDefaultAsync(x => x.GateId == id, cancellationToken);
        if (currentGate is null)
            return Result.Failure(GateErrors.GateNotFound);

        var UpdatedGate = gate.Adapt<Gate>();
        currentGate.GateType = UpdatedGate.GateType;
        currentGate.DeviceId = UpdatedGate.DeviceId;
        currentGate.Status = UpdatedGate.Status;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteGateAsync(int id, CancellationToken cancellationToken = default)
    {
        var Gate = await _context.Gates.FirstOrDefaultAsync(x => x.GateId == id, cancellationToken);
        if (Gate is null)
            return Result.Failure(GateErrors.GateNotFound);
        
        _context.Remove(Gate);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateGateStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentGate = await _context.Gates.FirstOrDefaultAsync(x => x.GateId == id, cancellationToken);

        if (currentGate is null)
            return Result.Failure(GateErrors.GateNotFound);

        if (currentGate.Status.Equals("Open", StringComparison.OrdinalIgnoreCase))
            currentGate.Status = "Closed";

        if (currentGate.Status.Equals("Closed", StringComparison.OrdinalIgnoreCase))
            currentGate.Status = "Open";
 
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
 
    public async Task<Result<IEnumerable<GateResponse>>> GetGatesByGarageIdAsync(int garageId, CancellationToken cancellationToken = default)
    {
        var garageExists = await _context.Garages.AnyAsync(g => g.GarageId == garageId, cancellationToken);

        if (!garageExists)
            return Result.Failure<IEnumerable<GateResponse>>(GateErrors.GarageNotFound);

        var GatesOfGarage = await _context.Gates.Where(g => g.GarageId == garageId).ToListAsync(cancellationToken);
        return Result.Success(GatesOfGarage.Adapt<IEnumerable<GateResponse>>());
    }
}
