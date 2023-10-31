using System.ComponentModel.DataAnnotations;
namespace Memory.Enums;

public enum FolderNames
{
    [Display(Name = "")] Default,
    [Display(Name = "Цитаты")] Quote,


    //Links
    [Display(Name = "Аниме, которые смотрел")] AnimeWatched = 100,
    [Display(Name = "Аниме, которые хочу посмотреть")] AnimeWillWatch,
    [Display(Name = "Сайты для просмотра аниме")] AnimeSites,
    [Display(Name = "Аниме клипы")] AnimeClips,

    [Display(Name = "Русская музыка, которую слушал")] RussianMusicListened = 110,
    [Display(Name = "Зарубежная музыка, которую слушал")] ForeignMusicListened,
    [Display(Name = "Музыка, которую хочу послушать")] MusicWillListen,
    [Display(Name = "Красивые песни")] BieutifulSongs,
    [Display(Name = "Сайты для прослушивания музыки")] MusicSites,
    [Display(Name = "Cсылки музыки")] MusicLinks,

    [Display(Name = "Фильмы, которые смотрел")] FilmsWatched = 120,
    [Display(Name = "Фильмы, которые хочу посмотреть")] FilmsWillWatch,
    [Display(Name = "Сайты для просмотра фильмов")] FilmSites,
    [Display(Name = "Фильмы, ломающие психику, которые смотрел")] FilmsBreakingPsycheWatched,
    [Display(Name = "Фильмы, ломающие психику, которые хочу посмотреть")] FilmsBreakingPsycheWillWatch,

    [Display(Name = "Сериалы, которые смотрел")] SerialsWatched = 130,
    [Display(Name = "Сериалы, которые хочу посмотреть")] SerialsWillWatch,
    [Display(Name = "Сайты для просмотра сериалов")] SerialSites,

    [Display(Name = "Красивые видео")] BieutifulVideos = 140,
    [Display(Name = "Nightcore")] NightcoreMusic,
    [Display(Name = "Необычная музыка")] UnusualMusic,
    [Display(Name = "Расслабляющая музыка")] RelaxingMusic,
    [Display(Name = "Концентрирующая музыка")] ConcentratingMusic,
    [Display(Name = "Ниточки")] Threads,

    [Display(Name = "Ссылки игр")] GameLinks = 160,
    [Display(Name = "Игры, в которые играл")] GamesPlayed = 162,

    [Display(Name = "Рецепты еды")] FoodRecipes = 170,
    [Display(Name = "Рецепты напитков")] DrinkRecipes,

    [Display(Name = "Расписание")] Schedule = 180,
    [Display(Name = "Текстовые заметки")] TextNotes,
    [Display(Name = "Youtube заметки")] YoutubeNotes,
    [Display(Name = "Ссылочные заметки")] LinkNotes,


    [Display(Name = "Халява")] Freebie = 200,

    [Display(Name = "Мультсериалы моего детства")] CartoonSeriesMyChildhood = 210,
    [Display(Name = "Мультфильмы, которые смотрел")] CartoonFilmsWatched,
    [Display(Name = "Мультсериалы, которые смотрел")] CartoonSeriesWatched,


    //Backend
    [Display(Name = "Полезные инструменты(backend)")] BackendUsefulTools = 500,
    [Display(Name = "Основа(C#)")] CSharpBase = 510,
    [Display(Name = "Статьи(C#)")] CSharpArticles = 511,
    [Display(Name = "Основа(SQL)")] SQLBase = 520,
    [Display(Name = "Основа(Blazor)")] BlazorBase = 530,
    [Display(Name = "Основа(ASP.NET Core)")] AspNetCoreBase = 540,
    [Display(Name = "Основа(Testing)")] TestingBase = 550,
    [Display(Name = "Основа(VCS)")] VersionControlSystemBase = 560,


    //Frontend
    [Display(Name = "Полезные инструменты(frontend)")] FrontendUsefulTools = 700,
    [Display(Name = "Готовые элементы")] FinishedElements = 701,
    [Display(Name = "Основа(HTML)")] HtmlBase = 710,
    [Display(Name = "Основа(CSS)")] CssBase = 720,
    [Display(Name = "Стили элементов(CSS)")] CssElementStyles = 721,
    [Display(Name = "Основа(TypeScript)")] TypeScriptBase = 730,
    [Display(Name = "Основа(JavaScript)")] JavaScriptBase = 740,


    //Mobile
    [Display(Name = "Полезные инструменты(mobile)")] MobileUsefulTools = 900,
    [Display(Name = "Основа(Xamarin)")] XamarinBase = 910,
}
