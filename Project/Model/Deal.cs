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

        [JsonProperty(PropertyName = "price")]
        public float SalePrice { get; set; }

        [JsonProperty(PropertyName = "retailPrice")]
        public float NormalPrice { get; set; }

        [JsonProperty(PropertyName = "savings")]
        public float SavingPercentage { get; set; }
    }
}