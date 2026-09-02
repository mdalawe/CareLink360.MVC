using CareLink360.Context;
using CareLink360.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class RatingRepository(ApplicationDbContext context) : IRatingRepository
{
    public async Task<Rating> CreateAsync(Rating rating)
    {
        await context.Ratings.AddAsync(rating);
        await context.SaveChangesAsync();
        return rating;
    }

    public async Task<List<Rating>> GetByCaregiverIdAsync(Guid caregiverId)
    {
        return await context.Ratings
            .Where(x => x.CaregiverId == caregiverId)
            .ToListAsync();
    }
}