using System;
using System.Collections.Generic;
using System.Text;

namespace FileSystemWatcher
{
    public class ResourceStringProvider : IStringProvider
    {
        System.Resources.ResourceManager _resourceManager;

        public ResourceStringProvider(string localization)
        {
            switch(localization)
            {
                case "ru":
                    _resourceManager = Properties.ResourcesRu.ResourceManager;
                    break;
                case "en":
                    _resourceManager = Properties.ResourcesEn.ResourceManager;
                    break;
                default:
                    _resourceManager = Properties.ResourcesEn.ResourceManager;
                    break;
            }
        }

        public string GetString(PhrasesEnum phrases)
        {
            return _resourceManager.GetString(phrases.ToString());
        }
    }
}
