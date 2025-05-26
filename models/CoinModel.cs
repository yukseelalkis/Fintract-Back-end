using Newtonsoft.Json;
using System;

namespace api.models
{
    public class CoinModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("current_price")]
        public double Current_Price { get; set; }

        [JsonProperty("market_cap")]
        public long? Market_Cap { get; set; }

        [JsonProperty("market_cap_rank")]
        public int? Market_Cap_Rank { get; set; }

        [JsonProperty("fully_diluted_valuation")]
        public long? Fully_Diluted_Valuation { get; set; }

        [JsonProperty("total_volume")]
        public long? Total_Volume { get; set; }

        [JsonProperty("high_24h")]
        public double? High_24h { get; set; }

        [JsonProperty("low_24h")]
        public double? Low_24h { get; set; }

        [JsonProperty("price_change_24h")]
        public double? Price_Change_24h { get; set; }

        [JsonProperty("price_change_percentage_24h")]
        public double? Price_Change_Percentage_24h { get; set; }

        [JsonProperty("market_cap_change_24h")]
        public double? Market_Cap_Change_24h { get; set; }

        [JsonProperty("market_cap_change_percentage_24h")]
        public double? Market_Cap_Change_Percentage_24h { get; set; }

        [JsonProperty("circulating_supply")]
        public double? Circulating_Supply { get; set; }

        [JsonProperty("total_supply")]
        public double? Total_Supply { get; set; }

        [JsonProperty("max_supply")]
        public double? Max_Supply { get; set; } =0.0;

        [JsonProperty("ath")]
        public double? Ath { get; set; }

        [JsonProperty("ath_change_percentage")]
        public double? Ath_Change_Percentage { get; set; }

        [JsonProperty("ath_date")]
        public DateTime? Ath_Date { get; set; }

        [JsonProperty("atl")]
        public double? Atl { get; set; }

        [JsonProperty("atl_change_percentage")]
        public double? Atl_Change_Percentage { get; set; }

        [JsonProperty("atl_date")]
        public DateTime? Atl_Date { get; set; }

        [JsonProperty("roi")]
        public object? Roi { get; set; }

        [JsonProperty("last_updated")]
        public DateTime? Last_Updated { get; set; }
    }
}
