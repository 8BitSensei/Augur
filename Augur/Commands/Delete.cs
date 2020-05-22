using LiteDbTest.Interfaces;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace LiteDbTest.Commands
{
    public class Delete : ICommand
    {
        private IConfigService _configService;
        private readonly CommandLineApplication app = new CommandLineApplication()
        {
            Description = "Creates files with the names of given strings and containing the coresponding hashcode.",
            ExtendedHelpText = "Following argument is required -strings and options are case sensitive."
        };

        public string ApplicationName { get { return "delete"; } }

        public Delete(IConfigService configService)
        {
            _configService = configService;

            app.HelpOption("-?|-h|--help");
            app.OnExecute(() =>
            {
                return 0;
            });
        }

        public int Execute(string[] args)
        {
            var result = app.Execute(args);
            return result;
        }
    }
}
