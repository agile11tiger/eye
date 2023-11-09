using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyE.Shared.ViewModels.Identity
{
    public class RefreshTokenViewModel
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public IEnumerable<string>? Messages { get; set; }
    }
}
