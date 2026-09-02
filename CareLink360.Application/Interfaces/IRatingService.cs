using CareLink360.Application.DTOs;
using CareLink360.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.Interfaces
{
    public interface IRatingService
    {
        Task<Rating> CreateAsync(CreateRatingDto dto);
        Task<List<Rating>> GetByCaregiverIdAsync(Guid caregiverId);
    }
}
