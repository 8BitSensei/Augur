namespace LiteDbTest.Interfaces
{
    public interface IScraperService
    {
        (string, string) ProcessData(string url);
    }
}