using System.ComponentModel.DataAnnotations;

namespace EyE.Client.Enums
{
    public enum FilterKeys
    {
        [Display(Name = "Начинаеться с")] StartWith,
        [Display(Name = "Содержит")] Contains,
        [Display(Name = "Дата выпуска")] StartingDateTime,
        [Display(Name = "Дата выпуска >")] StartingDateTimeMore,
        [Display(Name = "Дата выпуска <")] StartingDateTimeLess,
        [Display(Name = "Дата добавления")] AddingDateTime,
        [Display(Name = "Дата добавления >")] AddingDateTimeMore,
        [Display(Name = "Дата добавления <")] AddingDateTimeLess,
        [Display(Name = "Мой рейтинг")] MyRating,
        [Display(Name = "Мой рейтинг >")] MyRatingMore,
        [Display(Name = "Мой рейтинг <")] MyRatingLess,
        [Display(Name = "AniDb рейтинг")] AniDbRating,
        [Display(Name = "AniDb рейтинг >")] AniDbRatingMore,
        [Display(Name = "AniDb рейтинг <")] AniDbRatingLess,
        [Display(Name = "IMDb рейтинг")] IMDbRating,
        [Display(Name = "IMDb рейтинг >")] IMDbRatingMore,
        [Display(Name = "IMDb рейтинг <")] IMDbRatingLess,
    }
}
