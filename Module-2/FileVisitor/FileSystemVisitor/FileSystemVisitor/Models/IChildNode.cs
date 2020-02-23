namespace FileSystemVisitor.Models
{
    public interface IChildNode
    {
        FolderNode Parent { get; }
    }
}