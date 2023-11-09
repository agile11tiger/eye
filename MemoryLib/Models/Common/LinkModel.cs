using MemoryLib.Enums;
using MemoryLib.Models.Common.Interfaces;
namespace MemoryLib.Models.Common;

public class LinkModel : IDbFolderItem, IEquatable<LinkModel>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Link { get; set; }
    public string? ImageSource { get; set; }
    public FolderNames FolderName { get; set; }

    public bool Equals(LinkModel? other) => Id == other?.Id;
    public override bool Equals(object? obj) => Equals(obj as LinkModel);
    public override int GetHashCode() => Id;
}
