using EyE.Shared.Models.Common;
using EyE.Shared.Models.Review;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EyE.Client.Enums
{
    //В дальнейшем, по этим ключам, с помощью рефлексии, находят свойства
    public enum SortingKeys
    {
        [Display(Name = "Имени")] Name,
        [Display(Name = "Текста")] Text,
        [Display(Name = "Даты добавления")] AddingDate,
        [Display(Name = "Даты выпуска")] StartingDate,
        [Display(Name = "Мой рейтинг")] MyRating,
        [Display(Name = "AniDb рейтинг")] AniDbRating,
        [Display(Name = "IMDb рейтинг")] IMDbRating,
    }
}
