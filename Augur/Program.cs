using Autofac;
using LiteDB;
using LiteDbTest.Interfaces;
using LiteDbTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDbTest
{
    // TODO: add a --filter to run
    // TODO: insert single entry
    // TODO: update entry
    // TODO: output entry/ies
    // TODO: Delete entry

    public class Program
    {
        static void Main(string[] args)
        {
            using (var db = new LiteDatabase(@"C:\Temp\new.db"))
            {
                var container = new Container(db).container;
                var commands = container.Resolve<IEnumerable<ICommand>>();
                var configService = container.Resolve<IConfigService>();

                if (args.Length > 0)
                    commands.Where(x => x.ApplicationName.Equals(args[0]))?.FirstOrDefault()?.Execute(args?.Skip(1)?.ToArray());
                else
                {
                    Console.WriteLine(" run -- Scrape RIB content for entities");
                    Console.WriteLine(" export -- Export data as a csv node and edge list");
                    Console.WriteLine(" reset -- Reset your index and start scraping RIB content from 0");
                    Console.WriteLine(" delete -- Delete a table");
                }
            }
        }
    }
}
