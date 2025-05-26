
using api.Dtos.Stocks;
using api.models;
using System;
using System.Linq;

namespace api.Mappers
{
    public static class CoinMarketChartMapper
    {
        public static CoinMarketChartDto ToCoinMarketChartDto(this CoinMarketChart chart)
        {
            return new CoinMarketChartDto
            {
                Prices = chart.Prices.Select(p => new ChartDataDto
                {
                    Date = DateTimeOffset.FromUnixTimeMilliseconds((long)p[0]).UtcDateTime,
                    Value = p[1]
                }).ToList(),

                MarketCaps = chart.MarketCaps.Select(m => new ChartDataDto
                {
                    Date = DateTimeOffset.FromUnixTimeMilliseconds((long)m[0]).UtcDateTime,
                    Value = m[1]
                }).ToList(),

                TotalVolumes = chart.TotalVolumes.Select(v => new ChartDataDto
                {
                    Date = DateTimeOffset.FromUnixTimeMilliseconds((long)v[0]).UtcDateTime,
                    Value = v[1]
                }).ToList()
            };
        }
    }
}
