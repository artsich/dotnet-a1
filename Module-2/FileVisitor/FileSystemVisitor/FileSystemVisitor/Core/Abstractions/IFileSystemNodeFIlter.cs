using FileSystemVisitor.Models;
using System;
using System.Collections.Generic;

namespace FileSystemVisitor.Core.Abstractions
{
    public interface IFileSystemNodeFilter
    {
        IEnumerable<FileSystemNode> FilterBy(FolderNode root, Predicate<FileSystemNode> predicate);
    }
}
