using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class SubmitNinDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "NIN must be exactly 11 digits.")]
        public string Nin { get; set; } = string.Empty;
    }
}
