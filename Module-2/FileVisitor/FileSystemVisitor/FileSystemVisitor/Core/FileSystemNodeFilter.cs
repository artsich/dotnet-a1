using FileSystemVisitor.Core.Abstractions;
using FileSystemVisitor.Infrastructure;
using FileSystemVisitor.Infrastructure.Exceptions;
using FileSystemVisitor.Models;
using System;
using System.Collections.Generic;

namespace FileSystemVisitor.Core
{
    public class FileSystemNodeFilter : IFileSystemNodeFilter
    {
        private bool _shoudBeStopped;

        public event EventHandler<FileNodeFindEvent> FilteredFileFound;
        public event EventHandler<FolderNodeFindEvent> FilteredFolderFound;

        public IEnumerable<FileSystemNode> FilterBy(FolderNode root, Predicate<FileSystemNode> predicate)
        {
            var list = new LinkedList<FileSystemNode>();
            try
            {
                if (TryToAddChildNodes(root, predicate, list))
                {
                    list.AddFirst(root);
                }
            }
            catch (StopException) { }

            return list;
        }

        private bool TryToAddChildNodes(FolderNode root, Predicate<FileSystemNode> predicate, LinkedList<FileSystemNode> list)
        {
            var isChildAccording = false;
            foreach (var node in root)
            {
                if (_shoudBeStopped)
                {
                    throw new StopException();
                }

                var predicateResult = predicate(node);
                switch (node)
                {
                    case FileNode file:
                        {
                            if (predicateResult)
                            {
                                var ev = new FileNodeFindEvent(file);
                                FilteredFileFound?.Invoke(this, ev);

                                if (ev.ShouldBeAdd)
                                {
                                    list.AddLast(file);
                                    isChildAccording = true;
                                    _shoudBeStopped = ev.StopSearch;
                                }
                            }
                            break;
                        }
                    case FolderNode folder:
                        {
                            var lastNode = list.Last;

                            if (TryToAddChildNodes(folder, predicate, list) || predicateResult)
                            {
                                var ev = new FolderNodeFindEvent(folder);
                                FilteredFolderFound?.Invoke(this, ev);

                                if (ev.ShouldBeAdd)
                                {
                                    if (lastNode != null)
                                    {
                                        list.AddAfter(lastNode, folder);
                                    }
                                    else if(list.Count > 0)
                                    {
                                        list.AddFirst(folder);
                                    }
                                    else
                                    {
                                        list.AddLast(folder);
                                    }
                                    isChildAccording = true;
                                    _shoudBeStopped = ev.StopSearch;
                                }
                            }
                        }
                        break;
                }
            }
            return isChildAccording;
        }
    }
}
