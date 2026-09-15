using CareLink360.Application.DTOs;
using CareLink360.Context;
using CareLink360.Domain.Entities;
using CareLink360.Domain.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<ApplicationUser> _passwordHasher;

    public ClientService(
        IClientRepository repository,
        ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
        _passwordHasher =
            new PasswordHasher<ApplicationUser>();
    }


    public async Task<Client> CreateAsync(CreateClientDto dto)
    {
        var emailExists = await _context.Users
            .AnyAsync(x => x.Email == dto.Email);

        if (emailExists)
        {
            throw new Exception(
                "A user with this email already exists."
            );
        }

        using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = "Client",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password
                );

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var client = new Client
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                FullName =
                    $"{dto.FirstName} {dto.LastName}",
                Gender = dto.Gender,
                HomeAddress = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                City = dto.City,
                IsNinVerified = false
            };

            var result =
                await _repository.CreateAsync(client);

            await transaction.CommitAsync();

            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task<List<Client>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }



    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }




    public async Task<bool> UpdateAsync(
        Guid id,
        CreateClientDto dto)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
        {
            return false;
        }

        client.FirstName = dto.FirstName;
        client.LastName = dto.LastName;
        client.FullName =
            $"{dto.FirstName} {dto.LastName}";
        client.PhoneNumber = dto.PhoneNumber;
        client.City = dto.City;
        client.HomeAddress = dto.Address;

        await _context.SaveChangesAsync();

        return true;
    }



    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}