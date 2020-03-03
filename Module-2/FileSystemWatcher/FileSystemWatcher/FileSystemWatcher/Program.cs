using FileSystemWatcher.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

//TODO: Maybe check on cycle in folder path????
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
					//TODO: Add new string to resources.
					Console.WriteLine($"{e.Message}{_stringProvider.GetString(PhrasesEnum.NOT_FOUND_FOLDER)}");
				}

				Console.WriteLine(_stringProvider.GetString(PhrasesEnum.HELLO));
				while (Console.Read() != 'q') ;
			}
		}

		private void OnFileMoved(object sender, Events.FileMovedEvent args)
		{
			var builder = new StringBuilder();
			builder.Append(_stringProvider.GetString(PhrasesEnum.FILE_MOVED));
			builder.Append(" ");
			builder.Append(args.Name);
			builder.Append(" ");
			builder.Append(_stringProvider.GetString(PhrasesEnum.FROM));
			builder.Append(" ");
			builder.Append(args.From);
			builder.Append(" ");
			builder.Append(_stringProvider.GetString(PhrasesEnum.TO));
			builder.Append(" ");
			builder.Append(args.To);
			builder.Append(" ");
			Console.WriteLine(builder.ToString());
		}

		private void OnFileAdded(object sender, Events.FileAddedEvent args)
		{
			Console.WriteLine("On file added");
		}

		private void OnPatternMatchResult(object sender, Events.PatternMatchEvent args)
		{
			Console.WriteLine("On pattern succes or fail");
		}

		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();
			var appSettings = config.GetSection("AppSettings").Get<Setting>();

			if (appSettings == null)
			{
				//TODO: MOVE TO RESOURCES
				Console.WriteLine("Sorry but (appsetting.json) file not found!!");
			}
			else
			{
				new Program(
					appSettings,
					new ResourceStringProvider(appSettings.Localization)
				).Run();
			}
		}
	}
}
