using EyE.Client.Enums;
using System.Collections.Generic;
using System.Linq;
namespace EyE.Client.Components;

public partial class Sorting
{
    [Parameter] public SortingViewModel Model { get; set; }
    [Parameter] public IEnumerable<object> Data { get; set; }
    [Parameter] public EventCallback<IEnumerable<object>> UpdateData { get; set; }
    [Parameter] public IDictionary<SortingKeys, string> SortingParameters { get; set; }
    public IDictionary<SortingKeys, string> GetSortingParameters() => SortingParameters ?? new Dictionary<SortingKeys, string>();

    /// <summary>
    /// Сортирует используя рефлексию
    /// </summary>
    private async Task Sort()
    {
        if (SortingParameters == default)
            return;

        var currentSortingParameter = Model.CurrentSortingParameter.ToString();
        var data = Model.CurrentSortingState == SortingStateKeys.Ascend
            ? Data.OrderBy(item => GetPropertyValue(item))
            : Data.OrderByDescending(item => GetPropertyValue(item));

        await UpdateData.InvokeAsync(data);
    }

    private object GetPropertyValue(object obj)
    {
        var propertyName = Model.CurrentSortingParameter.ToString();
        return obj.GetType().GetProperty(propertyName).GetValue(obj);
    }
}

public class SortingViewModel
{
    public SortingStateKeys CurrentSortingState { get; set; }
    public SortingKeys CurrentSortingParameter { get; set; }
}
