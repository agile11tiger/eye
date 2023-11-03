using MemoryClient.Enums;
using Memory.Models.Common;
using Memory.Models.Review;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace MemoryClient.Components;

public partial class Filter
{
    [Parameter] public FilterViewModel Model { get; set; }
    [Parameter] public IEnumerable<object> Data { get; set; }
    [Parameter] public EventCallback<IEnumerable<object>> UpdateData { get; set; }
    [Parameter] public IDictionary<FilterKeys, string> FilterParameters { get; set; }
    public IDictionary<FilterKeys, string> GetFilterParameters() => FilterParameters ?? new Dictionary<FilterKeys, string>();

    public async Task Filtrate()
    {
        if (string.IsNullOrEmpty(Model.Value) || FilterParameters == default)
            return;

        var data = GetFiltratedData();
        await UpdateData.InvokeAsync(data);
    }

    private IEnumerable<object> GetFiltratedData()
    {
        return (Data.FirstOrDefault()) switch
        {
            AnimeModel _ => GetFiltratedData(Data.Select(item => (AnimeModel)item)),
            IMDbModel _ => GetFiltratedData(Data.Select(item => (IMDbModel)item)),
            ReviewModel _ => GetFiltratedData(Data.Select(item => (ReviewModel)item)),
            LinkModel _ => GetFiltratedData(Data.Select(item => (LinkModel)item)),

            TextModel _ => GetFiltratedData(Data.Select(item => (TextModel)item)),
            _ => Enumerable.Empty<object>(),
        };
    }

    private IEnumerable<AnimeModel> GetFiltratedData(IEnumerable<AnimeModel> data)
    {
        var isNumber = double.TryParse(Model.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var number);
        var emptyScroll = Enumerable.Empty<AnimeModel>();

        return Model.Key switch
        {
            FilterKeys.AniDbRating => isNumber ? data.Where(item => item.AniDbRating == number) : emptyScroll,
            FilterKeys.AniDbRatingMore => isNumber ? data.Where(item => item.AniDbRating > number) : emptyScroll,
            FilterKeys.AniDbRatingLess => isNumber ? data.Where(item => item.AniDbRating < number) : emptyScroll,
            FilterKeys.MyRating => isNumber ? data.Where(item => item.MyRating == number) : emptyScroll,
            FilterKeys.MyRatingMore => isNumber ? data.Where(item => item.MyRating > number) : emptyScroll,
            FilterKeys.MyRatingLess => isNumber ? data.Where(item => item.MyRating < number) : emptyScroll,
            _ => GetFiltratedData((IEnumerable<ReviewModel>)data).Select(item => (AnimeModel)item),
        };
    }

    private IEnumerable<IMDbModel> GetFiltratedData(IEnumerable<IMDbModel> data)
    {
        var isNumber = double.TryParse(Model.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var number);
        var emptyScroll = Enumerable.Empty<IMDbModel>();

        return Model.Key switch
        {
            FilterKeys.IMDbRating => isNumber ? data.Where(item => item.IMDbRating == number) : emptyScroll,
            FilterKeys.IMDbRatingMore => isNumber ? data.Where(item => item.IMDbRating > number) : emptyScroll,
            FilterKeys.IMDbRatingLess => isNumber ? data.Where(item => item.IMDbRating < number) : emptyScroll,
            _ => GetFiltratedData((IEnumerable<ReviewModel>)data).Select(item => (IMDbModel)item),
        };
    }

    private IEnumerable<ReviewModel> GetFiltratedData(IEnumerable<ReviewModel> data)
    {
        var isDateTime = DateTime.TryParse(Model.Value, out var dateTime);
        var emptyScroll = Enumerable.Empty<ReviewModel>();

        return Model.Key switch
        {
            FilterKeys.StartingDateTime => isDateTime ? data.Where(item => item.StartingDate == dateTime) : emptyScroll,
            FilterKeys.StartingDateTimeMore => isDateTime ? data.Where(item => item.StartingDate > dateTime) : emptyScroll,
            FilterKeys.StartingDateTimeLess => isDateTime ? data.Where(item => item.StartingDate < dateTime) : emptyScroll,
            FilterKeys.AddingDateTime => isDateTime ? data.Where(item => item.AddingDate == dateTime) : emptyScroll,
            FilterKeys.AddingDateTimeMore => isDateTime ? data.Where(item => item.AddingDate > dateTime) : emptyScroll,
            FilterKeys.AddingDateTimeLess => isDateTime ? data.Where(item => item.AddingDate < dateTime) : emptyScroll,
            _ => GetFiltratedData((IEnumerable<LinkModel>)data).Select(item => (ReviewModel)item),
        };
    }

    private IEnumerable<LinkModel> GetFiltratedData(IEnumerable<LinkModel> data)
    {
        return Model.Key switch
        {
            FilterKeys.StartWith => data.Where(item => item.Name.StartsWith(Model.Value, StringComparison.InvariantCultureIgnoreCase)),
            FilterKeys.Contains => data.Where(item => item.Name.Contains(Model.Value, StringComparison.InvariantCultureIgnoreCase)),
            _ => Enumerable.Empty<LinkModel>(),
        };
    }

    private IEnumerable<TextModel> GetFiltratedData(IEnumerable<TextModel> data)
    {
        return Model.Key switch
        {
            FilterKeys.StartWith => data.Where(item => item.Text.StartsWith(Model.Value, StringComparison.InvariantCultureIgnoreCase)),
            FilterKeys.Contains => data.Where(item => item.Text.Contains(Model.Value, StringComparison.InvariantCultureIgnoreCase)),
            _ => Enumerable.Empty<TextModel>(),
        };
    }
}

public class FilterViewModel
{
    public FilterKeys Key { get; set; }
    public string Value { get; set; }
}
