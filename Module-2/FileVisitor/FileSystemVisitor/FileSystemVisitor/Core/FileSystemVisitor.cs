using FileSystemVisitor.Infrastructure;
using FileSystemVisitor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;

namespace FileSystemVisitor.Core
{
	public class FileSystemVisitor
	{
		public FolderNode RootNode { get; private set; }
		private Predicate<FileSystemNode> _filterBy;

		private bool _searchIsStopped;

		public event EventHandler StartHandler;
		public event EventHandler EndHandler;

		public event EventHandler<FileNodeFindEvent> FileFound;
		public event EventHandler<FolderNodeFindEvent> FolderFound;

		public event EventHandler<FileNodeFindEvent> FilteredFileFound;
		public event EventHandler<FolderNodeFindEvent> FilteredFolderFound;

		public IEnumerable<FileSystemNode> FilterBy(Predicate<FileSystemNode> predicate)
		{
			yield return RootNode;
			var iter = Dfs(RootNode, predicate);
			foreach(var item in iter)
			{
				yield return item;
			}
		}

		private IEnumerable<FileSystemNode> Dfs(FolderNode root, Predicate<FileSystemNode> predicate)
		{
			foreach(var node in root)
			{
				switch (node)
				{
					case FileNode file:
						if (predicate(file))
						{
							yield return file;
						}
						break;
					case FolderNode folder:
						var childResult = Dfs(folder, predicate);

						if (predicate(folder)) yield return folder;
						var child = childResult.FirstOrDefault();
						if (child != null)
						{
							yield return folder;
							yield return child;
						}

						break;
				}
			}
		}

		public void Start(string root, Predicate<FileSystemNode> filterBy)
		{
            _filterBy = filterBy;
			SetUpTree(root);
		}

		private void SetUpTree(string root)
		{
			if (!Directory.Exists(root))
			{
				throw new DirectoryNotFoundException(root);
			}

			var rootDirInfo = new DirectoryInfo(root);

			StartHandler?.Invoke(this, EventArgs.Empty);
			_searchIsStopped = false;
			VisitFolder(rootDirInfo, null);
			_searchIsStopped = true;
			EndHandler?.Invoke(this, EventArgs.Empty);
		}

		private bool VisitFileSystemInfo(FolderNode rootFolder, FileSystemInfo info)
		{
			switch(info.Attributes)
			{
				case FileAttributes.Archive:
					return VisitFile((FileInfo)info, rootFolder);
				case FileAttributes.Directory:
					return VisitFolder((DirectoryInfo)info, rootFolder);
			}

			return true;
		}

		private bool VisitFile(FileInfo fileInfo, FolderNode rootNode)
		{
			var file = Map(fileInfo, rootNode);
			var filterResult = _filterBy == null ? true : _filterBy.Invoke(file);

			if (filterResult)
			{
				var fEvent = new FileNodeFindEvent(file);
				FileFound?.Invoke(this, fEvent);
				ProcessEvent(fEvent);

				if (fEvent.ShouldBeAdd)
				{
					rootNode.Add(file);
					return true;
				}
			}

			return false;
		}

		private bool VisitFolder(DirectoryInfo dirInfo, FolderNode rootNode = null)
		{
			var folder = Map(dirInfo, rootNode);

			if (rootNode == null)
			{
				RootNode = folder;
			}

			var childFilterResult = false;
			foreach (var item in dirInfo.EnumerateFileSystemInfos())
			{
				var validChild = VisitFileSystemInfo(folder, item);
				childFilterResult = childFilterResult || validChild;
			}

			var filterResult = _filterBy == null ? true : _filterBy.Invoke(folder);
			if (filterResult || childFilterResult)
			{
				var fEvent = new FolderNodeFindEvent(folder);
				FolderFound?.Invoke(this, fEvent);
				ProcessEvent(fEvent);

				if(fEvent.ShouldBeAdd)
				{
					rootNode?.Add(folder);
					return true;
				}
			}

			return false;
		}

		private void ProcessEvent(FileSystemNodeEvent _event)
		{
			_searchIsStopped = _event.StopSearch;
		}

		private FileNode Map(FileInfo fileInfo, FolderNode rootNode)
		{
			return new FileNode (
				rootNode,
				fileInfo.FullName,
				fileInfo.Name,
				fileInfo.Extension,
				fileInfo.Length
			);
		}

		private FolderNode Map(DirectoryInfo dir, FolderNode rootNode)
		{
			return new FolderNode (
				rootNode,
				dir.FullName,
				dir.Name
			);
		}
	}
}