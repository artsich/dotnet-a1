using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Crawler.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler
{
    public class HtmlCrawlerSetting
    {
        public string RootUri { get; set; }

        public string SaveDir { get; set; }

        public int MaxDeep { get; set; }
    }

    public class HtmlCrawler
    {
        private HtmlParser htmlParser = new HtmlParser();
        private IDataProvider _dataProvider;

        private HtmlCrawlerSetting _setting;

        private HashSet<Uri> _downloadedUri = new HashSet<Uri>();

        public HtmlCrawler(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IList<ParsedElement> Parse(HtmlCrawlerSetting setting, string resourcePath)
        {
            _setting = setting;
            return ParseHtml(new Uri(setting.RootUri));
        }

        private IList<ParsedElement> ParseHtml(Uri uri)
        {
            if (SaveUri(uri))
            {
                var htmlCode = _dataProvider.GetFrom(uri.ToString()).Result;

                var document = htmlParser.ParseDocument(htmlCode);

                var parsedElement = new ParsedElement()
                {
                    Content = htmlCode,
                    BaseName = uri.AbsolutePath.Length == 0 ? "index.html" : uri.AbsolutePath,
                    BaseFolder = _setting.SaveDir
                };

                var result = VisitTagA(document, uri);
                result.Add(parsedElement);
                return result;
            }

            return null;
        }

        private IList<ParsedElement> VisitTagA(IHtmlDocument doc, Uri baseUri)
        {
            IList<ParsedElement> result = new List<ParsedElement>();
            var tags = doc.QuerySelectorAll("a");

            foreach (var a in tags)
            {
                var href = a.GetAttribute("href");

                if (!string.IsNullOrEmpty(href))
                {
                    var downloadFrom = FormatPath(href);

                    if (downloadFrom == null) 
                    {
                        continue;
                    }

                    var downloads = ParseHtml(downloadFrom);
                    if (downloads != null)
                    {
                        result = result.Union(downloads).ToList();
                    }

                    a.SetAttribute("href", Path.Combine(_setting.SaveDir, downloadFrom.AbsolutePath));
                }
            }

            return result;
        }

        private bool SaveUri(Uri uri)
        {
            if (!_downloadedUri.Contains(uri))
            {
                _downloadedUri.Add(uri);
                return true;
            }
            return false;
        }

        private Uri FormatPath(string path)
        {
            if (path.Contains("javascript:")) { return null; }

            if (path.StartsWith("//"))
            {
                return new Uri($"http:{path}");
            }
            else
            {
                if (path.StartsWith("/"))
                {
                    return new Uri($"{_setting.RootUri}{path}");
                }
                else
                {
                    return new Uri($"{_setting.RootUri}/{path}");
                }
            }
        }

        private bool IsCurrentDomain(string url)
        {
            return url.StartsWith(_setting.RootUri);
        }

        private void SetToProp(string name, object sourceObj, object value)
        {
            sourceObj.GetType().GetProperty(name).SetValue(sourceObj, value);
        }

        private T GetPropValue<T>(string name, object sourceObj)
            where T : class
        {
            return sourceObj.GetType().GetProperty(name).GetValue(sourceObj) as T;
        }
    }
}