using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class Store
    {
        [JsonProperty(PropertyName = "storeID")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "steam")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "banner")]
        public string BannerUrl { get; set; }

        [JsonProperty(PropertyName = "logo")]
        public string LogoUrl { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string IconUrl { get; set; }
    }
}
