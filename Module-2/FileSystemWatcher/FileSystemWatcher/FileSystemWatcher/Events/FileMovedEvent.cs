namespace FileSystemWatcher.Events
{
    public class FileMovedEvent
    {

        public string Name { get; }

        public string From { get; }

        public string To { get; }

        public FileMovedEvent(string name, string from, string to)
        {
            Name = name;
            To = to;
            From = from;
        }
    }
}
