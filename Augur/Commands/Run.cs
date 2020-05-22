using LiteDbTest.Interfaces;
using LiteDbTest.Models;
using Microsoft.Extensions.CommandLineUtils;

namespace LiteDbTest.Commands
{
    public class Run : ICommand
    {
        private IDataService _dataService;
        private IScraperService _scraperService;
        private IConfigService _configService;
        private CommandLineApplication app = new CommandLineApplication()
        {
            Description = "Creates files with the names of given strings and containing the coresponding hashcode.",
            ExtendedHelpText = "Following argument is required -strings and options are case sensitive."
        };

        public string ApplicationName { get { return "run"; } }

        public Run(IDataService dataService, IScraperService scraperService, IConfigService configService)
        {
            _dataService = dataService;
            _scraperService = scraperService;
            _configService = configService;
            var currentState = configService.GetConfig();

            app.HelpOption("-?|-h|--help");
            var filter = app.Option("-f|--filter <string>", "Filter results", CommandOptionType.MultipleValue);

            app.OnExecute(() =>
            {
                if (filter.HasValue())
                {
                    for (var i = currentState.lastIndex + 1; i <= 3550; i++)
                    {
                        var output = _scraperService.ProcessData("https://romaninscriptionsofbritain.org/inscriptions/" + i);
                        if (output.Item1 != string.Empty)
                            _dataService.GetArtifact(output.Item1, output.Item2);

                        _configService.SetConfig(new Config { lastIndex = i });
                    }

                    return 1;
                }
                else
                {
                    for (var i = currentState.lastIndex + 1; i <= 3550; i++)
                    {
                        var output = _scraperService.ProcessData("https://romaninscriptionsofbritain.org/inscriptions/" + i);
                        if (output.Item1 != string.Empty)
                            _dataService.GetArtifact(output.Item1, output.Item2);

                        _configService.SetConfig(new Config { lastIndex = i });
                    }

                    return 1;
                }
            });
        }

        public int Execute(string[] args)
        {
            var result = app.Execute(args);
            return result;
        }
    }
}
