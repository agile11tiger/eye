using MemoryLib.Enums;
namespace MemoryLib.Helpers;

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
