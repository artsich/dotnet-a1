using FileSystemWatcher.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileSystemWatcher
{
    public class FileWatcher : IFileWatcher
    {
        private int _orderNumber;
        private IList<Rule> _rules;

        private System.IO.FileSystemWatcher _fileSystemWatcher;

        public EventHandler<Events.FileAddedEvent> FileAdded;
        public EventHandler<Events.FileMovedEvent> FileMoved;
        public EventHandler<Events.PatternMatchEvent> PatternMatchResult;

        public FileWatcher(IList<Rule> rules)
        {
            _rules = rules;
            _fileSystemWatcher = new System.IO.FileSystemWatcher();
        }

        public void StartWatch(string[] paths)
        {
            _fileSystemWatcher.Changed += OnFileSystemWatcherChanged;
        }

        private void OnFileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
        }

        private void CheckRuleForFile(FileInfo fileInfo)
        {
            _rules.FirstOrDefault(rule => {
                var result = Regex.Match(fileInfo.Name, rule.Pattern).Success;
                if (result)
                {
                    var name = IsNeedToModifyFileName(fileInfo, rule.ModifyRule);
                    var destPath = rule != null ? rule.DestinationPath : rule.DefaultPath;
                    MoveFile(fileInfo.Name, fileInfo.FullName, destPath + name);
                    PatternMatchResult?.Invoke(this, new Events.PatternMatchEvent(result, fileInfo.Name));
                }
                return result;
            });
        }

        private string IsNeedToModifyFileName(FileInfo fileInfo, ModifyRule rule)
        {
            var name = fileInfo.Name;
            
            if (rule.IsOrderNumberAdd)
            {
                name += _orderNumber++;
            }

            if (rule.IsDateAdded)
            {
                //TODO: Check how work with date.
            }

            return name;
        }

        private void MoveFile(string name, string from, string to)
        {
            File.Move(from, to);
            FileMoved?.Invoke(this, new Events.FileMovedEvent(name, from, to));
        }
    }
}
