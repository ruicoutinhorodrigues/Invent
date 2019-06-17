using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invent.Common.Models
{
    public class Test
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
