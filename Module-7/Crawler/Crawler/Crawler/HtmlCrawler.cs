using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Crawler.Core;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Crawler
{
    public class HtmlCrawler
    {
        private HtmlParser htmlParser = new HtmlParser();
        private IDataProvider _dataProvider;

        public HtmlCrawler(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<ParsedElement[]> ParseFromUrl(string baseUri, string resourceName, string saveBase)
        {
            var htmlCode = await _dataProvider.GetFrom(Path.Combine(baseUri, resourceName));

            var document = htmlParser.ParseDocument(htmlCode);

            var el = new ParsedElement
            {
                BaseFolder = saveBase,
                BaseName = resourceName,
                Content = htmlCode
            };

            VisitTagA(document, el);

            return await Task.FromResult<ParsedElement[]>(null);
        }

        private IList<ParsedElement> VisitTagA(IHtmlDocument doc, ParsedElement parentNode)
        {
            IList<ParsedElement> result = new List<ParsedElement>();

            var tags = doc.QuerySelectorAll("a");

            foreach (var a in tags)
            {
                var href = a.GetAttribute("href");
                var origin = GetPropValue<string>("Origin", a);

                if (!string.IsNullOrEmpty(href))
                {
                    var parsedEl = ParseFromUrl(baseUri, href, destFolder, href);

                    //var newHref = Path.Combine(destFolder, a.);

                    a.SetAttribute("Href", Path.Combine());
                }
            }

            return result;
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