using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invent.Common.Models.Local
{
    public class LocalLocation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
