using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stocks
{
    public class CoinMarketChartDto
    {
        public List<ChartDataDto> Prices { get; set; }
        public List<ChartDataDto> MarketCaps { get; set; }
        public List<ChartDataDto> TotalVolumes { get; set; }
    }

    public class ChartDataDto
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}