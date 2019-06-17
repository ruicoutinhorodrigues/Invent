using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invent.Common.Models
{
    public class Selection
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageFullPath")]
        public string ImageFullPath { get; set; }
    }
}
