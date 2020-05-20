using CommandLine;
using Crawler.Core;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    class Program
    {
        public CommandLineOptions Options;
       
        public Program(CommandLineOptions options)
        {
            Options = options;
        }

        public async Task Start()
        {
            var crawler = new HtmlCrawler(new HttpProvider());
            var result = crawler.Parse(new HtmlCrawlerSetting()
            {
                MaxDeep = Options.AnalyzeDeep,
                RootUri = @"http://ru.simplesite.com/",
                SaveDir = Options.DestinationFolder
            }, "");

            return;
            
            Console.Write("Enter url: ");
            var url = Console.ReadLine();
            
            //var parsedElements = await crawler.ParseFromUrl(url, Options.DestinationFolder);
            //Directory.CreateDirectory(Options.DestinationFolder);
            //foreach(var el in parsedElements)
            {
                //await File.WriteAllBytesAsync(el.SavePath, el.Content);
            }
        }

        static async Task Main(string[] args)
        {
            await Task.Run(() => Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(async options =>
                {
                    try
                    {
                        await new Program(options).Start();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                })
            );
        }
    }
}