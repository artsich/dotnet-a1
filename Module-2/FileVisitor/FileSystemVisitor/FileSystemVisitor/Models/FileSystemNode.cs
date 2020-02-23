namespace FileSystemVisitor.Models
{
    public abstract class FileSystemNode
    {
        public string Name { get; set; }

        public string Path { get; set; }
      
        public virtual long Size { get; protected set; }
    }
}