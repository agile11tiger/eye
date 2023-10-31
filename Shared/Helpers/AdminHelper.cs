using Memory.Enums;
namespace Memory.Helpers;

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
