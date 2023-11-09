using EyE.Shared.Models.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyE.Shared.Models.Common
{
    public class ResponseModel: IDatabaseItem
    {
        public int Id { get; set; }
        public IEnumerable<string>? Messages { get; set; }
    }
}
