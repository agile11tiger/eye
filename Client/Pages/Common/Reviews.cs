using EyE.Client.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EyE.Client.Pages.Common
{
    public class Reviews<T> : Folders<T> where T: ReviewModel, new()
    {
        public readonly Dictionary<SortingKeys, string> SortingParameters = new()
        {
            { SortingKeys.Name, SortingKeys.Name.GetAttribute<DisplayAttribute>().Name },
            { SortingKeys.StartingDate, SortingKeys.StartingDate.GetAttribute<DisplayAttribute>().Name },
            { SortingKeys.AddingDate, SortingKeys.AddingDate.GetAttribute<DisplayAttribute>().Name },
        };

        public readonly Dictionary<FilterKeys, string> FilterParameters = new()
        {
            { FilterKeys.StartWith, FilterKeys.StartWith.GetAttribute<DisplayAttribute>().Name },
            { FilterKeys.Contains, FilterKeys.Contains.GetAttribute<DisplayAttribute>().Name },
            { FilterKeys.StartingDateTime, FilterKeys.StartingDateTime.GetAttribute<DisplayAttribute>().Name },
            { FilterKeys.StartingDateTimeMore, FilterKeys.StartingDateTimeMore.GetAttribute<DisplayAttribute>().Name },
            { FilterKeys.StartingDateTimeLess, FilterKeys.StartingDateTimeLess.GetAttribute<DisplayAttribute>().Name },
            { FilterKeys.AddingDateTime, FilterKeys.AddingDateTime.GetAttribute<DisplayAttribute>().Name },
            { FilterKeys.AddingDateTimeMore, FilterKeys.AddingDateTimeMore.GetAttribute<DisplayAttribute>().Name },
            { FilterKeys.AddingDateTimeLess, FilterKeys.AddingDateTimeLess.GetAttribute<DisplayAttribute>().Name },
        };

    }
}
