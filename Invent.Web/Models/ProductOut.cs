using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Models
{
    public class ProductOut
    {
        public string ReferenceCode { get; set; }

        public decimal? Value { get; set; }

        public string ImageFullPath { get; set; }

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
