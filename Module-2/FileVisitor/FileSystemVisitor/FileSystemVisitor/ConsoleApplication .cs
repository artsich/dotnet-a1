using FileSystemVisitor.Core;
using FileSystemVisitor.Models;
using System;

namespace FileSystemVisitor
{
	public class ConsoleApplication : BaseApplication
	{
		private const string FolderPath = @"D:\dotnet-a1\Module-2\FileVisitor\FileSystemVisitor\FileSystemVisitor\bin\Debug\netcoreapp3.1\Test";

		public ConsoleApplication()
		{
			BuildTree(FolderPath, node => node.Name.Contains("e"));
		}

		protected override void FileVisitorOnStartHandler(object sender, EventArgs e)
		{
			Console.WriteLine("Start processing..");
		}

		protected override void FileVisitorOnEndHandler(object sender, EventArgs e)
		{
			Console.WriteLine("End processing..");
			Console.WriteLine("FS structure:");
			PrintFileSystem(base.FolderNode);
		}

		private static void PrintFileSystem(FolderNode folderNode, string shift = "")
		{
			foreach (var item in folderNode)
			{
				switch (item)
				{
					case FileNode file:
						Console.WriteLine($"{shift}{file.Name}");
						break;
					case FolderNode folder:
						Console.WriteLine($"{shift}-{folder.Name}");
						PrintFileSystem(folder, shift + '\t');
						break;
				}
			}
		}
	}
}