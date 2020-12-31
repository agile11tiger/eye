using System.ComponentModel.DataAnnotations;

namespace EyE.Shared.Enums
{
    public enum FolderNames
    {
        [Display(Name = "")] Default,
        [Display(Name = "Цитаты")] Quote,


        //Links
        [Display(Name = "Аниме, которые я смотрел")] AnimeWatched = 100,
        [Display(Name = "Аниме, которые я хочу посмотреть")] AnimeWillWatch,
        [Display(Name = "Сайты для просмотра аниме")] AnimeSites,

        [Display(Name = "Музыка, которую я слушал")] MusicListened = 110,
        [Display(Name = "Музыка, которую я хочу послушать")] MusicWillListen,
        [Display(Name = "Сайты для прослушивания музыки")] MusicSites,

        [Display(Name = "Фильмы, которые я смотрел")] FilmsWatched = 120,
        [Display(Name = "Фильмы, которые я хочу посмотреть")] FilmsWillWatch,
        [Display(Name = "Сайты для просмотра фильмов")] FilmSites,
        [Display(Name = "Фильмы, ломающие психику, которые смотрел")] FilmsBreakingPsycheWatched,
        [Display(Name = "Фильмы, ломающие психику, которые хочу посмотреть")] FilmsBreakingPsycheWillWatch,

        [Display(Name = "Сериалы, которые я смотрел")] SerialsWatched = 130,
        [Display(Name = "Сериалы, которые я хочу посмотреть")] SerialsWillWatch,
        [Display(Name = "Сайты для просмотра сериалов")] SerialSites,

        [Display(Name = "Видео, которые цепляют")] BieutifulVideos = 140,
        [Display(Name = "Nightcore")] Nightcore,

        [Display(Name = "Рецепты еды")] FoodRecipes = 150,
        [Display(Name = "Рецепты напитков")] DrinkRecipes,


        //Backend
        [Display(Name = "Полезные инструменты(backend)")] BackendUsefulTools = 500,
        [Display(Name = "Основа(C#)")] CSharpBase = 510,
        [Display(Name = "Статьи(C#)")] CSharpArticles = 511,
        [Display(Name = "Основа(SQL)")] SQLBase = 520,
        [Display(Name = "Статьи(SQL)")] SQLArticles = 521,
        [Display(Name = "Основа(Blazor)")] BlazorBase = 530,
        [Display(Name = "Статьи(Blazor)")] BlazorArticles = 531,


        //Frontend
        [Display(Name = "Полезные инструменты(frontend)")] FrontendUsefulTools = 700,
        [Display(Name = "Основа(HTML)")] HtmlBase = 710,
        [Display(Name = "Основа(CSS)")] CssBase = 720,
        [Display(Name = "Стили элементов(CSS)")] CssElementStyles = 721,
        [Display(Name = "Основа(TypeScript)")] TypeScriptBase = 730,
        [Display(Name = "Основа(JavaScript)")] JavaScriptBase = 740,


        //Mobile
        [Display(Name = "Полезные инструменты(mobile)")] MobileUsefulTools = 900,
        [Display(Name = "Основа(Xamarin)")] XamarinBase = 910,


        //Text
        [Display(Name = "Любимые песни")] FavoriteSongs = 1100,
    }
}
