
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace Smart_Parking_Garage.Services;

public class GateService(ApplicationDbContext context) : IGateService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Gate>> GetAllGatesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Gates.ToListAsync(cancellationToken);
    }


    public async Task<Gate?> GetGateByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Gates.FirstOrDefaultAsync(x => x.GateId == id, cancellationToken);
    }

    public async Task<Gate> CreateGateAsync(Gate gate, CancellationToken cancellationToken = default)
    {
        _context.Gates.Add(gate);
        await _context.SaveChangesAsync(cancellationToken);

        return gate;
    }


    public async Task<bool> UpdateGateAsync(int id, Gate gate, CancellationToken cancellationToken = default)
    {
        var currentGate = await GetGateByIdAsync(id, cancellationToken);
        if (currentGate == null)
            return false;

        currentGate.GateType = gate.GateType;
        currentGate.DeviceId = gate.DeviceId;
      
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> UpdateGateStatusAsync(int id, string status, CancellationToken cancellationToken = default)
    {
        var currentGate = await GetGateByIdAsync(id, cancellationToken);
        if (currentGate == null)
            return false;


        currentGate.Status = status;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteGateAsync(int id, CancellationToken cancellationToken = default)
    {
        var Gate = await GetGateByIdAsync(id, cancellationToken);
        if (Gate == null)
            return false;

        _context.Remove(Gate);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

 
}
