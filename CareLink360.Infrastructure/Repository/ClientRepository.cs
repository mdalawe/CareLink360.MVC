using CareLink360.Context;
using CareLink360.Domain.Entities;
using CareLink360.Domain.Interface;
using Microsoft.EntityFrameworkCore;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;

    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<Client> CreateAsync(Client client)
    {
        await _context.Clients.AddAsync(client);

        await _context.SaveChangesAsync();

        return client;
    }

    public async Task<Client?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<List<Client>> GetAllAsync()
    {
        return await _context.Clients
            .Include(c => c.User)
            .ToListAsync();
    }


    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }


    public async Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);

        await _context.SaveChangesAsync();
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
            return false;

        _context.Clients.Remove(client);

        await _context.SaveChangesAsync();

        return true;
    }
}