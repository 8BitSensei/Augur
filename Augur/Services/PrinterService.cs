using LiteDB;
using LiteDbTest.Interfaces;
using LiteDbTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LiteDbTest
{
    public class PrinterService : IPrinterService
    {
        private LiteDatabase _db;
        private IDataService _dataService;

        public PrinterService(LiteDatabase db, IDataService dataService)
        {
            _db = db;
            _dataService = dataService;
        }

        public void NodesToCsv()
        {
            var entities = _db.GetCollection<Entity>("entities").Query().ToArray();
            var csv = new StringBuilder();
            foreach (var entity in entities)
            {
                var newLine = string.Format("{0},{1}", entity.Name, entity.Occurences.Count);
                csv.AppendLine(newLine);
            }

            File.WriteAllText(@"C:\Temp\nodeList.csv", csv.ToString());
        }

        public void EdgesToCsv()
        {
            var entities = _db.GetCollection<Entity>("entities").Query().ToArray();
            var csv = new StringBuilder();
            var completedEntities = new List<string>();
            foreach (var entity in entities)
            {
                var connections = _dataService.GetEntityConections(entity.Name);
                foreach (var connection in connections.Item2)
                {
                    for (var i = 0; i < connection.weight; i++)
                    {
                        if (completedEntities.Contains(connection.name))
                            continue;

                        var newLine = string.Format("{0},{1}", entity.Name, connection.name);
                        csv.AppendLine(newLine);
                    }

                    completedEntities.Add(entity.Name);
                }
            }

            File.WriteAllText(@"C:\Temp\edgeList.csv", csv.ToString());
        }
    }
}
