using System;

namespace FileSystemWatcher.Events
{
    public class FileAddedEvent
    {
        public string FilePath { get; set; }

        public DateTime CreatedDate { get; }

        public FileAddedEvent(string filePath, DateTime createdDate)
        {
            FilePath = filePath;
            CreatedDate = createdDate;
        }
    }
}
