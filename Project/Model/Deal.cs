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
        [JsonProperty(PropertyName = "dealID")]
        public string dealId { get; set; }

        [JsonProperty(PropertyName = "storeID")]
        public string StoreId { get; set; }

        [JsonProperty(PropertyName = "gameID")]
        public string gameId { get; set; }

        [JsonProperty(PropertyName = "salePrice")]
        public float SalePrice { get; set; }

        [JsonProperty(PropertyName = "normalPrice")]
        public float NormalPrice { get; set; }

        [JsonProperty(PropertyName = "isOnSale")]
        public bool OnSale { get; set; }

        [JsonProperty(PropertyName = "steamRatingText")]
        public string SteamRatingText { get; set; }

        [JsonProperty(PropertyName = "steamRatingPercent")]
        public string SteamRatingPercent { get; set; }

        [JsonProperty(PropertyName = "steamRatingCount")]
        public string SteamReviewCount { get; set; }

        [JsonProperty(PropertyName = "releaseDate")]
        public int StartDate { get; set; } //in unix timestamp format

        [JsonProperty(PropertyName = "lastChange")]
        public int EndDate { get; set; } //in unix timestamp format

        [JsonProperty(PropertyName = "dealRating")]
        public float Rating { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string ImageUrl { get; set; }
    }
}
