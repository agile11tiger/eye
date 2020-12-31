using System.Collections.Generic;

namespace EyE.Client.Enums
{
    public enum SortingStateKeys
    {
        Ascend,
        Descend,
    }

    public static class SortingStates
    {
        public static readonly Dictionary<SortingStateKeys, string> Default = new Dictionary<SortingStateKeys, string>()
        {
            { SortingStateKeys.Ascend, "По увеличению" },
            { SortingStateKeys.Descend, "По уменьшению" }
        };
    }
}
