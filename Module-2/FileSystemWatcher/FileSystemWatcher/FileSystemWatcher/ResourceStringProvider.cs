using System.Globalization;
using System.Threading;

namespace FileSystemWatcher
{
	public class ResourceStringProvider : IStringProvider
	{
		System.Resources.ResourceManager _resourceManager;

		public ResourceStringProvider(string localization)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(localization);
			_resourceManager = Properties.Resource.ResourceManager;
		}

		public string GetString(PhrasesEnum phrases)
		{
			return _resourceManager.GetString(phrases.ToString());
		}
	}
}
