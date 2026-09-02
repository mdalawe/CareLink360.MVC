using CareLink360.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLik360.Domain.Interface
{
    public interface INinVerificationRepository
    {
        Task AddAsync(NinVerification verification);

        Task<NinVerification?> GetByIdAsync(Guid id);

        Task UpdateAsync(NinVerification verification);
    }
}
