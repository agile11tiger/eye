using EyE.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyE.Shared.Models.Identity
{
    public class LoginResponseModel : IdentityResponseModel
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
