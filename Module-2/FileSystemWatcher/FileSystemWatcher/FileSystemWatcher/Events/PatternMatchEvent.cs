namespace FileSystemWatcher.Events
{
    public class PatternMatchEvent
    {
        public bool IsSuccess { get; }

        public string FileName { get; set; }

        public PatternMatchEvent(bool isSuccess, string fileName)
        {
            IsSuccess = isSuccess;
            FileName = fileName;
        }
    }
}
