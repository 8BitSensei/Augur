using LiteDbTest.Models;

namespace LiteDbTest.Interfaces
{
    public interface IConfigService
    {
        Config GetConfig();
        Config SetConfig(Config config);
    }
}