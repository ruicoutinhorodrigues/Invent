﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invent.Common.Models
{
    public class TokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiration")]
        public string Expiration { get; set; }
    }
}
