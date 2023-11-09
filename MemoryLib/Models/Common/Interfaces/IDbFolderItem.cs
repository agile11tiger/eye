using IdentityLib.Models;
using MemoryLib.Enums;
namespace MemoryLib.Models.Common.Interfaces;

public interface IDbFolderItem : IDatabaseItem
{
    public FolderNames FolderName { get; set; }
}
