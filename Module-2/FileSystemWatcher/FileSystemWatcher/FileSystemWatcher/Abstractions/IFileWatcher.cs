namespace FileSystemWatcher.Abstractions
{
    public interface IFileWatcher
    {
        void StartWatch(string[] paths);
    }
}
