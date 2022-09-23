using System;

namespace EyE.Shared.Models.Common
{
    public class WikiModel : LinkModel
    {
        public string? WikiId { get; set; }
        public string? OriginalImageSource { get; set; }
        public string? Information { get; set; }
        public DateTime AddingDate { get; set; }
    }
}
