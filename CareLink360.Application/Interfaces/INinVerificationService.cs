using CareLink360.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.Interfaces
{
    public interface INinVerificationService
    {
        Task<object> SubmitAsync(SubmitNinDto dto);

        Task ReviewAsync(ReviewNinDto dto);
    }
}
