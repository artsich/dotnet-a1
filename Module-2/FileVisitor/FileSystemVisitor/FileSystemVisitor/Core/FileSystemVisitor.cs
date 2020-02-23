using FileSystemVisitor.Infrastructure;
using FileSystemVisitor.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitor.Core
{
	public class FileSystemVisitor
	{
		class StopSearchException : Exception
		{
		}

		public FolderNode RootNode { get; private set; }
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

		public IEnumerable<FileSystemNode> Filter(Predicate<FileSystemNode> predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			foreach (var item in RootNode)
			{
				if (!predicate(item)) continue;

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

			RootNode = new FolderNode()
			{
				Name = rootDirInfo.Name,
				Path = rootDirInfo.FullName,
			};

			StartHandler?.Invoke(this, EventArgs.Empty);

			try
			{
				FindNodesInFolder(RootNode, rootDirInfo);
			}
			catch (StopSearchException) { }

			EndHandler?.Invoke(this, EventArgs.Empty);
		}

		private void FindNodesInFolder(FolderNode rootNode, DirectoryInfo rootInfo)
		{
			var systemEntries = rootInfo.EnumerateFileSystemInfos();
			foreach (var entries in systemEntries)
			{
				var fsEvent = default(FileSystemNodeEvent);

				switch (entries.Attributes)
				{
					case FileAttributes.Archive:
						var fileInfo = (FileInfo)entries;
						var fileNode = new FileNode(
							fileInfo.FullName,
							fileInfo.Name,
							fileInfo.Extension,
							fileInfo.Length);

						var fileEvent = new FileNodeFindEvent(fileNode);

						FileFound?.Invoke(fileInfo, fileEvent);

						if (fileEvent.ShouldBeAdd)
						{
							rootNode.Add(fileNode);
						}
						fsEvent = fileEvent;
						break;
					case FileAttributes.Directory:
						var folderNode = new FolderNode
						{
							Name = entries.Name,
							Path = entries.FullName,
							Parent = rootNode
						};

						var folderEvent = new FolderNodeFindEvent(folderNode);
						FolderFound?.Invoke(this, folderEvent);

						if (folderEvent.ShouldBeAdd)
						{
							rootNode.Add(folderNode);
							FindNodesInFolder(folderNode, (DirectoryInfo)entries);
						}

						fsEvent = folderEvent;
						break;
				}

				if (fsEvent != null && fsEvent.StopSearch)
				{
					throw new StopSearchException();
				}
			}
		}
	}
}