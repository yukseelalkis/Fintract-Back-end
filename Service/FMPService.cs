using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.Interfaces;
using api.Mappers;
using api.models;
using Newtonsoft.Json;

namespace api.Service
{
    /// <summary>
    /// Financial Modeling Prep (FMP) API'si üzerinden şirket profil verilerini çeken servis.
    /// </summary>
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        /// <summary>
        /// FMPService için HttpClient ve IConfiguration bağımlılıkları alınır.
        /// </summary>
        /// <param name="httpClient">HTTP isteklerini göndermek için kullanılan istemci</param>
        /// <param name="config">Appsettings içindeki FMP API Key'e erişim sağlar</param>
        public FMPService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        /// <summary>
        /// Sembol (örneğin: "AAPL") bilgisine göre FMP API'den şirket profili çeker.
        /// </summary>
        /// <param name="symbol">Şirketin borsa sembolü (örneğin: "MSFT", "GOOG")</param>
        /// <returns>Stock modeline dönüştürülmüş FMP verisi veya null</returns>
        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                var result = await _httpClient.GetAsync(
                    $"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = tasks.FirstOrDefault();

                    if (stock != null)
                    {
                        return stock.toStockFMP(); // DTO -> Model dönüşümü
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[FMPService] Hata: {e.Message}");
            }

            return null;
        }
    }
}
