using FileSystemWatcher.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileSystemWatcher
{
	public class FileWatcher : IFileWatcher, IDisposable
	{
		private bool _isDisposed;
		private int _orderNumber;
		private string _defaultPath;
		private IList<Rule> _rules;

		private System.IO.FileSystemWatcher[] _watchers;

		public EventHandler<Events.FileAddedEvent> FileAdded;
		public EventHandler<Events.FileMovedEvent> FileMoved;
		public EventHandler<Events.PatternMatchEvent> PatternMatchResult;

		public FileWatcher(string defaultPath, IList<Rule> rules)
		{
			_rules = rules;
			_defaultPath = defaultPath;

			if (!Directory.Exists(_defaultPath))
			{
				Directory.CreateDirectory(_defaultPath);
			}
		}

		public void StartWatch(string[] paths)
		{
			var notExistedPath = paths.FirstOrDefault(path => !Directory.Exists(path));
			if (!string.IsNullOrEmpty(notExistedPath))
			{
				throw new DirectoryNotFoundException(notExistedPath);
			}

			_watchers = new System.IO.FileSystemWatcher[paths.Length];
			for (int i = 0; i < paths.Length; ++i)
			{
				_watchers[i] = new System.IO.FileSystemWatcher();
				_watchers[i].NotifyFilter = NotifyFilters.LastAccess
									 | NotifyFilters.LastWrite
									 | NotifyFilters.FileName;
				_watchers[i].Created += OnFileSystemWatcherChanged;
				_watchers[i].Path = paths[i];
				_watchers[i].EnableRaisingEvents = true;
			}
		}

		private void OnFileSystemWatcherChanged(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Created)
			{
				var matchedRule = CheckRuleForNameFile(e.Name);
				string modifyedName = e.Name;
				string destPath = _defaultPath;

				if (matchedRule != null)
				{
					modifyedName = ModifyNameIfNeeded(e.Name, matchedRule.ModifyRule);
					destPath = matchedRule.DestinationPath;
				}

				var newFilePath = Path.Combine(destPath, modifyedName);
				MoveFile(modifyedName, e.FullPath, newFilePath);

				PatternMatchResult?.Invoke(this, new Events.PatternMatchEvent(matchedRule != null, newFilePath));
			}
		}

		private Rule CheckRuleForNameFile(string fileName) => _rules.FirstOrDefault(rule => Regex.Match(fileName, rule.Pattern).Success);

		private string ModifyNameIfNeeded(string name, ModifyRule rule)
		{
			var result = name;
			if (rule.IsOrderNumberAdd)
			{
				result += _orderNumber++;
			}

			if (rule.IsDateAdded)
			{
				result += DateTimeOffset.Now;
			}

			return result;
		}

		private void MoveFile(string name, string from, string to)
		{
			File.Move(from, to, true);
			FileMoved?.Invoke(this, new Events.FileMovedEvent(name, from, to));
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				for (int i = 0; i < _watchers.Length; ++i)
				{
					_watchers[i].Created -= OnFileSystemWatcherChanged;
					_watchers[i].EnableRaisingEvents = false;
					_watchers[i].Dispose();
				}
				Array.Clear(_watchers, 0, _watchers.Length);
				_isDisposed = true;
			}
		}
	}
}
