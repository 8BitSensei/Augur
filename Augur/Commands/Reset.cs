using LiteDbTest.Interfaces;
using LiteDbTest.Models;
using Microsoft.Extensions.CommandLineUtils;

namespace LiteDbTest.Commands
{
    public class Reset : ICommand
    {
        private IConfigService _configService;
        private readonly CommandLineApplication app = new CommandLineApplication()
        {
            Description = "Creates files with the names of given strings and containing the coresponding hashcode.",
            ExtendedHelpText = "Following argument is required -strings and options are case sensitive."
        };

        public string ApplicationName { get { return "reset"; } }

        public Reset(IConfigService configService)
        {
            _configService = configService;

            app.HelpOption("-?|-h|--help");
            app.OnExecute(() =>
            {
                _configService.SetConfig(new Config() { lastIndex = 0 });
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
