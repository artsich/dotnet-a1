using FileSystemVisitor.Infrastructure;
using FileSystemVisitor.Models;
using System;
using System.IO;

namespace FileSystemVisitor.Core
{
	public class FileSystemVisitor
	{
		private FolderNode _rootNode;
		public FolderNode RootNode => _rootNode;

		private Predicate<FileSystemNode> _filterBy;

		private bool _searchIsStopped;

		public event EventHandler StartHandler;
		public event EventHandler EndHandler;

		public event EventHandler<FileNodeFindEvent> FileFound;
		public event EventHandler<FolderNodeFindEvent> FolderFound;

		public FolderNode Start(string root, Predicate<FileSystemNode> filterBy)
		{
			_filterBy = filterBy;
			SetUpTree(root);
			return _rootNode;
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
			if (_searchIsStopped)
			{
				return false;
			}

			switch (info.Attributes)
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
			if (_searchIsStopped)
			{
				return false;
			}

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
			if (_searchIsStopped)
			{
				return false;
			}

			var folder = Map(dirInfo, rootNode);

			if (rootNode == null)
			{
				_rootNode = folder;
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

				if (fEvent.ShouldBeAdd)
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
			return new FileNode(
				rootNode,
				fileInfo.FullName,
				fileInfo.Name,
				fileInfo.Extension,
				fileInfo.Length
			);
		}

		private FolderNode Map(DirectoryInfo dir, FolderNode rootNode)
		{
			return new FolderNode(
				rootNode,
				dir.FullName,
				dir.Name
			);
		}
	}
}