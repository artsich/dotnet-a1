using System.Text;
using FileSystemVisitor.Models;

namespace FileSystemVisitor.Infrastructure
{
    public abstract class FileSystemNodeEvent
    {
        public bool StopSearch { get; set; }

        public bool ShouldBeAdd { get; set; } = true;
    }
}