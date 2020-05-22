namespace LiteDbTest.Interfaces
{
    public interface ICommand
    {
        string ApplicationName { get; }
        int Execute(string[] args);
    }
}