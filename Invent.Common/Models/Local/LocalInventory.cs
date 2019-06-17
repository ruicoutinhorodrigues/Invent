using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invent.Common.Models.Local
{
    public class LocalInventory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        
        public string ImageFullPath { get; set; }
    }
}
