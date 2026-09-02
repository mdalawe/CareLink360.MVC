using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CareLink360.Domain.Entities;

public interface IRatingRepository
{
    Task<Rating> CreateAsync(Rating rating);
    Task<List<Rating>> GetByCaregiverIdAsync(Guid caregiverId);
}