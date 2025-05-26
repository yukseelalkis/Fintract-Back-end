using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api.models
{
    public class CoinMarketChart
    {
        [JsonProperty("prices")]
        public List<List<double>> Prices { get; set; }

        [JsonProperty("market_caps")]
        public List<List<double>> MarketCaps { get; set; }

        [JsonProperty("total_volumes")]
        public List<List<double>> TotalVolumes { get; set; }
    }
}