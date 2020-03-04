using FileSystemVisitor.Models;

namespace FileSystemVisitor.Infrastructure
{
    //TODO: Ask about Node: add to base class or not??
    public class FileNodeFindEvent : FileSystemNodeEvent
    {
        public FileNode File { get; }

        public FileNodeFindEvent(FileNode file)
        {
            File = file;
        }
    }
}