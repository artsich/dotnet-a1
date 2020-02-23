using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FileSystemVisitor.Models
{
    public class FolderNode : FileSystemNode, IChildNode, ICollection<FileSystemNode>
    {
        public int Count => _childrens.Count;

        public bool IsReadOnly { get; } = false;
        
        public FolderNode Parent { get; set; }

        private readonly IList<FileSystemNode> _childrens = new List<FileSystemNode>();

        public override long Size => _childrens.Sum(n => n.Size);

        public IEnumerator<FileSystemNode> GetEnumerator()
        {
            foreach (var child in _childrens)
            {
                yield return child;
                
                if (!(child is FolderNode folderNode)) continue;

                foreach (var node in folderNode)
                {
                    yield return node;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(FileSystemNode item)
        {
            _childrens.Add(item);
        }

        public void Clear()
        {
            _childrens.Clear();
        }

        public bool Contains(FileSystemNode item)
        {
            return _childrens.Contains(item);
        }

        public void CopyTo(FileSystemNode[] array, int arrayIndex)
        {
            _childrens.CopyTo(array, arrayIndex);
        }

        public bool Remove(FileSystemNode item)
        {
            return _childrens.Remove(item);
        }
    }
}