using CoreProject.Settings;

namespace CoreProject.DataAccess.Context
{
    public class MongoContext : IContext
    {
        public MongoSetting _setting;

        public MongoContext(MongoSetting setting)
        {
            _setting = setting;
        }
    }
}
