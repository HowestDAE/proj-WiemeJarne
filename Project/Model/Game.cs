using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class Game
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public Deal[] Deals { get; set; }

        [JsonIgnore]
        public int ReleaseDate { get; set; } //in unix timestamp format

        [JsonIgnore]
        public string Publisher { get; set; }
    }
}