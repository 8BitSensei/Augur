using LiteDbTest.Interfaces;
using LiteDbTest.Models;
using Microsoft.Extensions.CommandLineUtils;

namespace LiteDbTest.Commands
{
    public class Export : ICommand
    {
        private IPrinterService _printerService;
        private readonly CommandLineApplication app = new CommandLineApplication()
        {
            Description = "Creates files with the names of given strings and containing the coresponding hashcode.",
            ExtendedHelpText = "Following argument is required -strings and options are case sensitive."
        };

        public string ApplicationName { get { return "export"; } }

        public Export(IPrinterService printerService)
        {
            _printerService = printerService;

            app.HelpOption("-?|-h|--help");
            app.OnExecute(() =>
            {
                _printerService.NodesToCsv();
                _printerService.EdgesToCsv();
                return 1;
            });
        }

        public int Execute(string[] args)
        {
            var result = app.Execute(args);
            return result;
        }
    }
}
