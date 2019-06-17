using Newtonsoft.Json;
using System;

namespace Invent.Common.Models
{
    public class Product
    {
        [JsonProperty("referenceCode")]
        public string ReferenceCode { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("imageFullPath")]
        public string ImageFullPath { get; set; }

        public byte[] ImageArray { get; set; }

        [JsonProperty("inventoryName")]
        public string InventoryName { get; set; }

        [JsonProperty("inventoryManagerName")]
        public string InventoryManagerName { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("productModel")]
        public string ProductModel { get; set; }

        [JsonProperty("supplier")]
        public string Supplier { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("entryDate")]
        public DateTime EntryDate { get; set; }

        [JsonProperty("lastChangeDate")]
        public DateTime LastChangeDate { get; set; }

    }
}
