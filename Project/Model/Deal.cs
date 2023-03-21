using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class Deal
    {
        [JsonProperty(PropertyName = "storeID")]
        public string StoreId { get; set; }

        [JsonProperty(PropertyName = "savings")]
        public float SavingPercentage { get; set; }

        [JsonProperty(PropertyName = "salePrice")]
        public float SalePrice { get; set; }

        [JsonProperty(PropertyName = "normalPrice")]
        public float NormalPrice { get; set; }

        [JsonProperty(PropertyName = "steamRatingText")]
        public string SteamRatingText { get; set; }

        [JsonProperty(PropertyName = "steamRatingPercent")]
        public string SteamRatingPercent { get; set; }

        [JsonProperty(PropertyName = "steamRatingCount")]
        public string SteamReviewCount { get; set; }

        [JsonProperty(PropertyName = "releaseDate")]
        public int MetaCriticScore { get; set; }
    }
}