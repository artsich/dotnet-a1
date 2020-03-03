using FileSystemWatcher.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FileSystemWatcher
{
    class Program
    {
        private readonly Setting _setting;
        private readonly FileWatcher _fileWatcher;
        private readonly IStringProvider _stringProvider;

        private Program(Setting setting, IStringProvider stringProvider)
        {
            _setting = setting;
            _stringProvider = stringProvider;
            _fileWatcher = new FileWatcher(_setting.Rules);

            _fileWatcher.FileAdded += OnFileAdded;
            _fileWatcher.FileMoved += OnFileMoved;
            _fileWatcher.PatternMatchResult += OnPatternMatchResult;
        }

        //TODO: Optimize this pattern;
        private void OnFileMoved(object sender, Events.FileMovedEvent args)
        {
            var message = string.Format($"" +
                $"{_stringProvider.GetString(PhrasesEnum.FILE_MOVED)} " +
                $"{args.Name} " +
                $"{_stringProvider.GetString(PhrasesEnum.FROM)} " +
                $"{args.From} " +
                $"{_stringProvider.GetString(PhrasesEnum.TO)}",
                args.To
            );

            Console.WriteLine(message);
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

            if (appSettings != null)
            {
                Console.WriteLine("Sorry but (appsetting.json) file not found!!");
            }
            else
            {
                new Program(
                    appSettings,
                    new ResourceStringProvider(appSettings.Localization)
                );
            }
        }
    }
}
