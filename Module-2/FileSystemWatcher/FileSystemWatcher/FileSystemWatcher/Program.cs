using FileSystemWatcher.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FileSystemWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var appSettings = config.GetSection("AppSettings").Get<Setting>();
            Console.WriteLine($"Localization: {appSettings.Localization}");
        }
    }
}
