﻿using FileSystemVisitor.Models;
using System;

namespace FileSystemVisitor.Core
{
	public abstract class BaseApplication
	{
		private FileSystemVisitor _fileVisitor;

		protected FolderNode FolderNode => _fileVisitor.RootNode;

		protected BaseApplication()
		{
			_fileVisitor = new FileSystemVisitor();
			_fileVisitor.StartHandler += FileVisitorOnStartHandler;
			_fileVisitor.EndHandler += FileVisitorOnEndHandler;
			
			_fileVisitor.FileFound += FileVisitorOnFileFound;
			_fileVisitor.FolderFound += FileVisitorOnFolderFound;
			
			_fileVisitor.FilteredFileFound += FileVisitorOnFilteredFileFound;
			_fileVisitor.FilteredFolderFound += FileVisitorOnFilteredFolderFound;
		}

        protected void BuildTree(string root, Predicate<FileSystemNode> predicate)
        {
			_fileVisitor.Start(root, predicate);
        }

		protected virtual void FileVisitorOnFilteredFolderFound(object sender, Infrastructure.FolderNodeFindEvent e)
		{
		}

		protected virtual void FileVisitorOnFilteredFileFound(object sender, Infrastructure.FileNodeFindEvent e)
		{
		}

		protected virtual void FileVisitorOnFolderFound(object sender, Infrastructure.FolderNodeFindEvent e)
		{
		}

		protected virtual void FileVisitorOnFileFound(object sender, Infrastructure.FileNodeFindEvent e)
		{
		}

		protected virtual void FileVisitorOnStartHandler(object sender, EventArgs e)
		{
		}

		protected virtual void FileVisitorOnEndHandler(object sender, EventArgs e)
		{
		}
	}
}