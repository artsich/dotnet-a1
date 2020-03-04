using FileSystemWatcher.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace FileSystemWatcher
{
	class Program
	{
		private FileWatcher _fileWatcher;
		private readonly Setting _setting;
		private readonly IStringProvider _stringProvider;

		private Program(Setting setting, IStringProvider stringProvider)
		{
			_setting = setting;
			_stringProvider = stringProvider;
		}

		private void Run()
		{
			using (_fileWatcher = new FileWatcher(_setting.DefaultPath, _setting.Rules))
			{
				_fileWatcher.FileAdded += OnFileAdded;
				_fileWatcher.FileMoved += OnFileMoved;
				_fileWatcher.PatternMatchResult += OnPatternMatchResult;

				try
				{
					_fileWatcher.StartWatch(_setting.ListeningFolders);
				}
				catch (DirectoryNotFoundException e)
				{
					Console.WriteLine(PrepareMessage(PhrasesEnum.NOT_FOUND_FOLDER, e.Message));
					return;
				}

				Console.WriteLine(_stringProvider.GetString(PhrasesEnum.HELLO));
				while (Console.Read() != 'q') ;
			}
		}

		private void OnFileMoved(object sender, Events.FileMovedEvent args)
		{
			Console.WriteLine(PrepareMessage(PhrasesEnum.FILE_MOVED, args.Name, args.From, args.To));
		}

		private void OnFileAdded(object sender, Events.FileAddedEvent args)
		{
			Console.WriteLine(PrepareMessage(PhrasesEnum.FILE_ADDED, args.FilePath, args.CreatedDate));
		}

		private void OnPatternMatchResult(object sender, Events.PatternMatchEvent args)
		{
			Console.WriteLine(PrepareMessage(
				args.IsSuccess 
				? PhrasesEnum.PATTERN_RESULT_TRUE 
				: PhrasesEnum.PATTERN_RESULT_FALSE,
				args.FileName));
		}

		private string PrepareMessage(PhrasesEnum phrase, params object[] args)
		{
			var format = _stringProvider.GetString(phrase);
			return string.Format(format, args);
		}

		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();
			var appSettings = config.GetSection("AppSettings").Get<Setting>();
			var stringProvider = new ResourceStringProvider(appSettings.Localization);

			if (appSettings == null)
			{
				Console.WriteLine(stringProvider.GetString(PhrasesEnum.APP_SETTING_NOT_FOUND));
			}
			else
			{
				new Program(
					appSettings,
					stringProvider
				).Run();
			}
		}
	}
}
