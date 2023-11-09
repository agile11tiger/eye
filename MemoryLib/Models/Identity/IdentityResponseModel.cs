using EyE.Shared.Models.Common;
using EyE.Shared.Models.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyE.Shared.Models.Identity
{
    public abstract class IdentityResponseModel: ResponseModel
    {
        public string? UserId { get; set; }
    }
}
