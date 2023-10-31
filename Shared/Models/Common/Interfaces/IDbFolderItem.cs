using Identity.Models;
using Memory.Enums;
namespace Memory.Models.Common.Interfaces;

public interface IDbFolderItem : IDatabaseItem
{
    public FolderNames FolderName { get; set; }
}
