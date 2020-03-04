namespace FileSystemWatcher.Events
{
    public class PatternMatchEvent
    {
        public bool IsSuccess => MatchedRule != null;

        public string FileName { get; }

        public Rule MatchedRule { get;}

        public PatternMatchEvent(string fileName, Rule matchedRule)
        {
            FileName = fileName;
            MatchedRule = matchedRule;
        }
    }
}
