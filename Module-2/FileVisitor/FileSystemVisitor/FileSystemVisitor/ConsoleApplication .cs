using FileSystemVisitor.Core;
using FileSystemVisitor.Infrastructure;
using FileSystemVisitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileSystemVisitor
{
	public class ConsoleApplication : BaseApplication
	{
		public ConsoleApplication(string[] args)
		{
			if (args.Length > 0 && args[0].Length > 0)
			{
				BuildTree(args[0], node =>
				{
					if (node is FileNode fileNode)
					{
						return fileNode.Extension == ".java" || fileNode.Extension == ".py";
					}
					return true;
				});
			}
			else
			{
				Console.WriteLine("Run app like 'name.exe <path to folder>'");
			}
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

			PrintSizeOfBaseFolder();

			Script_1();
			Script_2();
		}

		protected override void FileVisitorOnFolderFound(object sender, FolderNodeFindEvent e)
		{
			if (e.Folder.Name == "folder2")
			{
				e.StopSearch = true;
			}
		}

		private static void PrintIterator(IEnumerable<FileSystemNode> iter)
		{
			var folder = iter.FirstOrDefault() as FolderNode;
			if (folder != null)
			{
				foreach (var item in iter)
				{
					var shift = Utils.CalcShiftLevel(item as IChildNode);
					Console.WriteLine($"{shift}{item.Name}");
				}
			}
			else
			{
				Console.WriteLine("\tNothing to print!!");
			}
		}

		private static void PrintFileSystem(FolderNode folderNode, string shift = "")
		{
			Console.WriteLine($"{shift}{folderNode.Name}");
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

		private void Script_1()
		{
			BaseScript("Filter by folder name := 'fol'", node =>
			{
				if (node is FolderNode folder)
				{
					return folder.Name.Contains("fol");
				}

				return false;
			});
		}

		private void Script_2()
		{
			BaseScript("Filter by file name := 'e'", node =>
			{
				if (node is FileNode file)
				{
					return file.Name.Contains('e');
				}

				return false;
			});
		}

		private void PrintSizeOfBaseFolder()
		{
			Console.WriteLine($"The size of base folder {base.FolderNode.Size}");
		}

		private void BaseScript(string name, Predicate<FileSystemNode> pred)
		{
			Console.WriteLine("------------");
			Console.WriteLine(name);
			PrintIterator(GetItems(pred));
			Console.WriteLine("------------");
		}
	}
}