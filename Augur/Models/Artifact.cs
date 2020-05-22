using System;
using System.Collections.Generic;
using System.Text;

namespace LiteDbTest.Models
{
    public class Artifact
    {
        public int id { get; set; }
        public Dictionary<int, string> Names { get; set; }
        public string Source { get; set; }
    }
}
