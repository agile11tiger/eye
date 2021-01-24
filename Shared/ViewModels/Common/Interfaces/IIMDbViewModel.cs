using System;
using System.Text.Json.Serialization;

namespace EyE.Shared.ViewModels.Common.Interfaces
{
    public interface IIMDbViewModel
    {
        public string Link { get; set; }
        public string IMDbId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string ImageSource { get; set; }
        public ushort Year { get; set; }
        public DateTime ReleasedDatetime { get; set; }
        public DateTime StartingDate { get; }
        public DateTime AddingDate { get; }
        public ushort Runtime { get; set; }
        public string Genre { get; set; }
        public string Information { get; set; }
        public double IMDbRating { get; set; }
        public int IMDbVotes { get; set; }
        public ushort TotalSeasons { get; set; }

        public bool IsError { get => !string.IsNullOrEmpty(Error); }
        public string Error { get; set; }
    }
}
