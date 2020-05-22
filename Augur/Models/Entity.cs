using System;
using System.Collections.Generic;
using System.Text;

namespace LiteDbTest.Models
{
    public class Entity
    {
        public int id { get; set; }
        public string Name { get; set; }
        public List<int> Occurences { get; set; }
    }
}
