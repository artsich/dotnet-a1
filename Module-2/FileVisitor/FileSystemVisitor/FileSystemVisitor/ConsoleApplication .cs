using FileSystemVisitor.Core;
using FileSystemVisitor.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FileSystemVisitor
{
	public class ConsoleApplication : BaseApplication
	{
		private const string FolderPath = @"D:\dotnet-a1\Module-2\FileVisitor\FileSystemVisitor\FileSystemVisitor\bin\Debug\netcoreapp3.1\Test";

		public ConsoleApplication()
		{
			BuildTree(FolderPath, node =>
			{
				if (node is FileNode fileNode)
				{
					//return fileNode.Extension == ".java" || fileNode.Extension == ".py";
				}
				return true;
			});
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


			Console.WriteLine("Filter built tree...");
			PrintIterator(GetItems(node =>
			{
				if (node is FileNode fileNode)
				{
					return fileNode.Extension == ".txt";
				}

				//if (node is FolderNode folder)
				//{
				//	return folder.Name.Contains("r2");
				//}

				return false;
			}));
		}

		private static void PrintIterator(IEnumerable<FileSystemNode> iter)
		{
			var folder = iter.FirstOrDefault() as FolderNode;
			if (folder != null)
			{
				foreach(var item in iter)
				{
					var shift = CalcShiftLevel(item as IChildNode);
					Console.WriteLine($"{shift}{item.Name}");
				}
			}
		}

		private static string CalcShiftLevel(IChildNode node, string shift = "")
		{
			if (node.Parent == null)
			{
				return shift;
			}

			return CalcShiftLevel(node.Parent, shift + '\t');
 		}

		private static void PrintFileSystem(FolderNode folderNode, string shift = "")
		{
			Console.WriteLine($"{shift}-{folderNode.Name}");
			foreach (var item in folderNode)
			{
				switch (item)
				{
					case FileNode file:
						Console.WriteLine($"{shift}\t{file.Name}");
						break;
					case FolderNode folder:
						PrintFileSystem(folder, shift + '\t');
						break;
				}
			}
		}
	}
}