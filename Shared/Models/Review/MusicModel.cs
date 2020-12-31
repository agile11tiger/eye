using EyE.Shared.Models.Common;

namespace EyE.Shared.Models.Review
{
    public class MusicModel : ReviewModel
    {
        public string DiscogsId { get; set; }
        public string Sites { get; set; }
        public string YoutubePlaylist { get; set; }
    }
}
