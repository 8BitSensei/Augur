using LiteDB;
using LiteDbTest.Interfaces;
using LiteDbTest.Models;
using System.Linq;

namespace LiteDbTest.Services
{
    public class ConfigService : IConfigService
    {
        private LiteDatabase _db;

        public ConfigService(LiteDatabase db)
        {
            _db = db;
        }

        public Config SetConfig(Config config)
        {
            var ent = _db.GetCollection<Config>("config");
            var result = GetConfig();
            result.lastIndex = config.lastIndex;
            ent.Update(result);
            return result;
        }

        public Config GetConfig()
        {
            var ent = _db.GetCollection<Config>("config");
            var result = ent.Query().ToArray().FirstOrDefault();
            if (result == null) 
            {
                ent.Insert(new Config() { lastIndex = 0 });
                return new Config() { lastIndex = 0 };
            }

            return result;
        }
    }
}
