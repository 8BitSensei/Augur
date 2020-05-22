using HtmlAgilityPack;
using LiteDB;
using LiteDbTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LiteDbTest
{
    public class ScraperService : IScraperService
    {
        private LiteDatabase _db;

        private readonly string[] _metaCategories = new string[]
        {
            "Type of object", "Material", "Dimensions", "Condition", "Decoration and iconography", "Site",
            "Find context", "Find date", "Modern location", "Institution/acc. #", "Style of lettering", "Letter heights",
            "Origin of text", "Document type", "Date", "Dating source"
        };

        public ScraperService(LiteDatabase db)
        {
            _db = db;
        }

        public (string, string) ProcessData(string url)
        {
            var doc = new HtmlWeb();
            var result = doc.Load(url);
            Console.WriteLine("url: " + url);
            var input = ScrapeRIB(result);
            if (!string.IsNullOrEmpty(input))
            {
                return (input, url);
            }

            Console.WriteLine();
            return (string.Empty, string.Empty);
        }

        private string ScrapeRIB(HtmlDocument result)
        {
            var meta = result.DocumentNode.SelectNodes("//dl[contains(@class, 'meta')]");
            var metaValues = new List<string>();
            foreach (var htmlNode in meta)
            {
                var test = htmlNode.InnerText.Split(_metaCategories, StringSplitOptions.RemoveEmptyEntries);
                foreach (var @value in test)
                    metaValues.Add(@value);
            }
            metaValues = metaValues.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();

            var translation = result.GetElementbyId("translation").InnerText.Split("Translation");
            translation = ProcessList(translation.ToList());

            var interpretive = result.DocumentNode.SelectNodes("//div[contains(@class, 'interpretive')]").FirstOrDefault()?.InnerText;
            interpretive = CleanLine(interpretive);

            var diplomatic = result.DocumentNode.SelectNodes("//div[contains(@class, 'diplomatic')]").FirstOrDefault()?.InnerText;
            diplomatic = CleanLine(diplomatic);

            var transcript = result.DocumentNode.SelectNodes("//div[contains(@class, 'transcript')]").FirstOrDefault()?.InnerText;
            transcript = CleanLine(transcript);

            var commentary = result.DocumentNode.SelectNodes("//*[(@id = \"commentary-notes\")]").FirstOrDefault()?.InnerText.Split("Commentary and notes");
            commentary = ProcessList(commentary.ToList());

            var bibliography = result.DocumentNode.SelectNodes("//*[(@id = \"bibliography\")]").FirstOrDefault()?.InnerText.Split("Bibliography");
            bibliography = ProcessList(bibliography.ToList());


            Console.WriteLine("diplomatic: " + diplomatic);
            Console.WriteLine("translation: " + (translation.Length > 0 ? translation[0] : "translation unavailable"));
            Console.WriteLine();
            Console.Write("Leave blank to make no entry: ");
            var input = Console.ReadLine();
            return input;
        }

        private string[] ProcessList(IList<string> input)
        {
            for (var i = 0; i < input.Count; i++)
            {
                var entry = input.ElementAt(i);
                entry = CleanLine(entry);
                input[i] = entry;
            }

            return input.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

        }

        private string CleanLine(string input)
        {
            var rgx4 = new Regex(@"[\r\n'/\\]");

            input = input.Replace("\n", " ");
            input = rgx4.Replace(input, string.Empty);
            input = input.Replace("  ", string.Empty);
            input = input.Replace("\"", string.Empty);

            return input;
        }
    }
}
