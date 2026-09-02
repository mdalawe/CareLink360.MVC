using CareLink360.Context;
using CareLink360.Domain.Entities;
using Microsoft.EntityFrameworkCore;

    public class JobRequestRepository(ApplicationDbContext context) : IJobRequestRepository
    {
        public async Task<JobRequest> CreateAsync(JobRequest request)
        {
            await context.JobRequests.AddAsync(request);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<JobRequest?> GetByIdAsync(Guid id)
        {
            return await context.JobRequests.FindAsync(id);
        }

        public async Task<List<JobRequest>> GetAllAsync()
        {
            return await context.JobRequests.ToListAsync();
        }

        public async Task UpdateAsync(JobRequest request)
        {
            context.JobRequests.Update(request);
            await context.SaveChangesAsync();
        }
    }

