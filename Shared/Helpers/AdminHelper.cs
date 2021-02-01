using EyE.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EyE.Shared.Helpers
{
    public static class AdminHelper
    {
        public readonly static FolderNames[] AdminFolders = new[]
        {
            FolderNames.Schedule,
            FolderNames.TextNotes,
            FolderNames.YoutubeNotes,
            FolderNames.LinkNotes,
            FolderNames.FilmsBreakingPsycheWatched,
            FolderNames.FilmsBreakingPsycheWillWatch,
        };
    }
}
