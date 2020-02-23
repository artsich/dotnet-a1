using System;
using System.Collections.Generic;
using System.Text;
using FileSystemVisitor.Core;
using FileSystemVisitor.Models;

namespace FileSystemVisitor
{
	public class ConsoleApplication : BaseApplication
	{
		public ConsoleApplication(string root, Predicate<FileSystemNode> predicate = null) : base(root, predicate)
		{
		}

		protected override void FileVisitorOnStartHandler(object sender, EventArgs e)
		{
			Console.WriteLine("Start processing..");
		}

		protected override void FileVisitorOnEndHandler(object sender, EventArgs e)
		{
			Console.WriteLine("End processing..");
			Console.WriteLine("FS structure");
			PrintFileSystem(base.FolderNode);
		}

		private static void PrintFileSystem(FolderNode folderNode, int shiftLevel = 0)
		{
			string shift = string.Join('\t', new char[shiftLevel]);

			foreach (var item in folderNode)
			{
				switch (item)
				{
					case FileNode file:
						Console.WriteLine($"{shift}{file.Name}");
						break;
					case FolderNode folder:
						Console.WriteLine($"-{shift}{folder.Name}");
						PrintFileSystem(folder, shiftLevel + 2);
						break;
				}
			}
		}
	}
}
