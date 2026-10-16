using CareLink360.Application.Interfaces;
using CareLink360.Context;
using CareLink360.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareLink360.Application.Services
{
    public class JobTypeService : IJobTypeService
    {
        private readonly ApplicationDbContext _context;

        public JobTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobType>> GetActiveJobTypesAsync()
        {
            return await _context.JobTypes
                .Where(j => j.IsActive)
                .OrderBy(j => j.Name)
                .ToListAsync();
        }
    }
}