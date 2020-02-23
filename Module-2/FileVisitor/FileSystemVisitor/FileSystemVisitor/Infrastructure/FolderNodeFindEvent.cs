using FileSystemVisitor.Models;

namespace FileSystemVisitor.Infrastructure
{
    public class FolderNodeFindEvent : FileSystemNodeEvent
    {
        public FolderNode Folder { get; }

        public FolderNodeFindEvent(FolderNode folder)
        {
            Folder = folder;
        }
    }
}