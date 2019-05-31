using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invent.Common.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("referenceCode")]
        public string ReferenceCode { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("entryDate")]
        public DateTime EntryDate { get; set; }

        [JsonProperty("lastChangeDate")]
        public DateTime LastChangeDate { get; set; }

        [JsonProperty("imageFullPath")]
        public string ImageFullPath { get; set; }

        [JsonProperty("inventoryId")]
        public long InventoryId { get; set; }

        [JsonProperty("locationId")]
        public long LocationId { get; set; }
   
        [JsonProperty("statusId")]
        public long StatusId { get; set; }
   
        [JsonProperty("categoryId")]
        public long CategoryId { get; set; }
   
        [JsonProperty("productModelId")]
        public long ProductModelId { get; set; }
   
        [JsonProperty("supplierId")]
        public long SupplierId { get; set; }
  
        [JsonProperty("sizeId")]
        public long? SizeId { get; set; }
      
        [JsonProperty("colorId")]
        public long? ColorId { get; set; }

        public override string ToString()
        {
            return $"{this.ReferenceCode} {this.Value}";
        }
    }
}
