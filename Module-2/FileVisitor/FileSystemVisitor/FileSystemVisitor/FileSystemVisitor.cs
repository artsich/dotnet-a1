using FileSystemVisitor.Infrastructure;
using FileSystemVisitor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace FileSystemVisitor
{
    public class FileSystemVisitor
    {
        private bool _stopSearch;
        private FolderNode _rootNode;
        private Predicate<FileSystemNode> _filterBy;

        public event EventHandler StartHandler;
        public event EventHandler EndHandler;

        public event EventHandler<FileNodeFindEvent> FileFound;
        public event EventHandler<FolderNodeFindEvent> FolderFound;

        public event EventHandler<FileNodeFindEvent> FilteredFileFound;
        public event EventHandler<FolderNodeFindEvent> FilteredFolderFound;

        public FileSystemVisitor(Predicate<FileSystemNode> filterBy)
        {
            _filterBy = filterBy;
        }

        public IEnumerable<FileSystemNode> Filter(Predicate<FileSystemNode> func)
        {
            foreach (var item in _rootNode)
            {
                if (!func(item)) continue;

                var ev = default(FileSystemNodeEvent);

                switch (item)
                {
                    case FileNode fileNode:
                        ev = new FileNodeFindEvent(fileNode);
                        FilteredFileFound?.Invoke(this, (FileNodeFindEvent)ev);
                        break;
                    case FolderNode folderNode:
                        ev = new FolderNodeFindEvent(folderNode);
                        FilteredFolderFound?.Invoke(this, (FolderNodeFindEvent)ev);
                        break;
                }

                if (ev != null && ev.StopSearch) break;
                if (!ev.ShouldBeAdd) continue;
                
                yield return item;
            }
        }

        public void Start(string root)
        {
            SetUpTree(root);
        }

        private void SetUpTree(string root)
        {
            if (!Directory.Exists(root))
            {
                throw new DirectoryNotFoundException(root);
            }

            var rootDirInfo = new DirectoryInfo(root);

            _rootNode = new FolderNode()
            {
                Name = rootDirInfo.Name,
                Path = rootDirInfo.FullName,
            };

            StartHandler?.Invoke(this, EventArgs.Empty);
            FindNodesInFolder(_rootNode, rootDirInfo);
            EndHandler?.Invoke(this, EventArgs.Empty);
        }

        private void FindNodesInFolder(FolderNode rootNode, DirectoryInfo rootInfo)
        {
            if (_stopSearch)
            {
                return;
            }

            var systemEntries = rootInfo.EnumerateFileSystemInfos();
            foreach (var entries in systemEntries)
            {
                var fileSystemEvent = default(FileSystemNodeEvent);
                var fileSystemNode = default(FileSystemNode);

                switch (entries.Attributes)
                {
                    case FileAttributes.Archive:
                        var fileInfo = (FileInfo)entries;
                        fileSystemNode = new FileNode(
                            fileInfo.FullName,
                            fileInfo.Name,
                            fileInfo.Extension,
                            fileInfo.Length);

                        fileSystemEvent = new FileNodeFindEvent((FileNode)fileSystemNode);
                        FileFound?.Invoke(fileInfo, (FileNodeFindEvent)fileSystemEvent);

                        break;
                    case FileAttributes.Directory:
                        fileSystemNode = new FolderNode
                        {
                            Name = entries.Name,
                            Path = entries.FullName,
                            Parent = rootNode
                        };

                        var fileNode = (FolderNode) fileSystemNode;
                        fileSystemEvent = new FolderNodeFindEvent(fileNode);
                        FolderFound?.Invoke(this, (FolderNodeFindEvent)fileSystemEvent);

                        if (fileSystemEvent.ShouldBeAdd)
                        {
                            rootNode.Add(fileSystemNode);
                            FindNodesInFolder(fileNode, (DirectoryInfo)entries);
                        }
                        else
                        {
                            //TODO: Ask George: do i need to add folder-files if isNeedToAdd-false ?
                        }
                        break;
                }

                if (fileSystemEvent.ShouldBeAdd)
                {
                    rootNode.Add(fileSystemNode);
                }

                if (fileSystemEvent.StopSearch)
                {
                    return;
                }
            }

        }
        private void VisitFolderNode(FolderNode folder)
        {

        }

        private void VisitFileNode(FileNode file)
        {

        }
    }
}