using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invent.Common.Models.Local
{
    public class LocalProduct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public long Value { get; set; }


        public string ImageFullPath { get; set; }

        public byte[] ImageArray { get; set; }


        public string InventoryName { get; set; }


        public string InventoryManagerName { get; set; }


        public string Location { get; set; }


        public string Status { get; set; }


        public string Category { get; set; }


        public string ProductModel { get; set; }


        public string Supplier { get; set; }


        public string Size { get; set; }

  
        public string Color { get; set; }

  
        public DateTime EntryDate { get; set; }


        public DateTime LastChangeDate { get; set; }
    }
}
