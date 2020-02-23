using System;
using System.Linq;
using FileSystemVisitor.Models;

namespace FileSystemVisitor
{
    public class Program
    {
        static void Main(string[] args)
        {
            var fileVisitor = new FileSystemVisitor(x => true);
            fileVisitor.StartHandler += FileVisitorOnStartHandler;
            fileVisitor.EndHandler += FileVisitorOnEndHandler;

            fileVisitor.FileFound += FileVisitorOnFileFound;
            fileVisitor.FolderFound += FileVisitorOnFolderFound;

            fileVisitor.FilteredFileFound += FileVisitorOnFilteredFileFound;
            fileVisitor.FilteredFolderFound += FileVisitorOnFilteredFolderFound;

            var result = fileVisitor.Filter(node =>
            {
                if (node is FileNode fileNode)
                {
                    return fileNode.Extension == "txt";
                }

                return false;
            });
        }

        private static void FileVisitorOnFilteredFolderFound(object sender, Infrastructure.FolderNodeFindEvent e)
        {
            throw new NotImplementedException();
        }

        private static void FileVisitorOnFilteredFileFound(object sender, Infrastructure.FileNodeFindEvent e)
        {
            throw new NotImplementedException();
        }

        private static void FileVisitorOnFolderFound(object sender, Infrastructure.FolderNodeFindEvent e)
        {
            throw new NotImplementedException();
        }

        private static void FileVisitorOnFileFound(object sender, Infrastructure.FileNodeFindEvent e)
        {
            throw new NotImplementedException();
        }

        private static void FileVisitorOnStartHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void FileVisitorOnEndHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
