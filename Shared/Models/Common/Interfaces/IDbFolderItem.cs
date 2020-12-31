using EyE.Shared.Enums;

namespace EyE.Shared.Models.Common.Interfaces
{
    public interface IDbFolderItem: IDatabaseItem
    {
        public FolderNames FolderName { get; set; }
    }
}
