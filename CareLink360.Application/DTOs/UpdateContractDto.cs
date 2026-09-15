using CareLink360.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class UpdateContractStatusDto
    {
        public Guid ContractId { get; set; }

        public ContractStatus Status { get; set; }
    }
}
