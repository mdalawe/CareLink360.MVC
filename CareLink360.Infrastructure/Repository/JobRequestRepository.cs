using CareLink360.Context;
using CareLink360.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class JobRequestRepository : IJobRequestRepository
{
    private readonly ApplicationDbContext context;

    public JobRequestRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<JobRequest> CreateAsync(JobRequest request)
    {
        await context.JobRequests.AddAsync(request);
        await context.SaveChangesAsync();

        return request;
    }

    public async Task<JobRequest?> GetByIdAsync(Guid id)
    {
        return await context.JobRequests
            .Include(x => x.Client)
            .Include(x => x.Caregiver)
            .Include(x => x.JobType)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<JobRequest>> GetAllAsync()
    {
        return await context.JobRequests
            .Include(x => x.Client)
            .Include(x => x.Caregiver)
            .Include(x => x.JobType)
            .OrderByDescending(x => x.RequestedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(JobRequest request)
    {
        context.JobRequests.Update(request);
        await context.SaveChangesAsync();
    }
}
