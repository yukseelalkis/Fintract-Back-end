using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.Interfaces;
using api.Mappers;
using api.models;
using Newtonsoft.Json;

namespace api.Service
{
    /// <summary>
    /// CoinGecko API üzerinden kripto para verilerini alan servis sınıfıdır.
    /// Kripto para detayları, popüler coinler ve grafik verileri çekilir.
    /// </summary>
    public class CoinService : ICoinService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// HTTP istekleri için HttpClient bağımlılığı alınır.
        /// </summary>
        /// <param name="httpClient">DI tarafından enjekte edilen HttpClient nesnesi</param>
        public CoinService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        }

        /// <summary>
        /// Tüm coin verilerini listeler (market cap'e göre sıralı ilk 20).
        /// </summary>
        /// <returns>CoinModelDto listesi</returns>
        public async Task<List<CoinModelDto>> GetAllAsync()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=20&page=1&sparkline=false");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var coinList = JsonConvert.DeserializeObject<List<CoinModel>>(content);
                    if (coinList != null)
                    {
                        return coinList.Select(x => x.ToCoinDto()).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[GetAllAsync] Hata: {e.Message}");
            }

            return new List<CoinModelDto>();
        }

        /// <summary>
        /// En popüler 3 coini döndürür (market cap'e göre).
        /// </summary>
        /// <returns>En çok ilgi gören 3 coin</returns>
        public async Task<List<CoinModelDto>> GetPopularCoinAsync()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=3&page=1");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var coinList = JsonConvert.DeserializeObject<List<CoinModel>>(content);
                    if (coinList != null)
                    {
                        return coinList.Select(x => x.ToCoinDto()).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[GetPopularCoinAsync] Hata: {e.Message}");
            }

            return new List<CoinModelDto>();
        }

        /// <summary>
        /// Belirli bir coinin son 7 günlük grafik verilerini döndürür.
        /// </summary>
        /// <param name="id">Coin ID (örnek: "bitcoin")</param>
        /// <returns>7 günlük CoinMarketChartDto verisi</returns>
        public async Task<CoinMarketChartDto> Get7DaysStatistics(string id)
        {
            var result = await _httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency=usd&days=7");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var chartData = JsonConvert.DeserializeObject<CoinMarketChart>(content);
                if (chartData != null)
                {
                    return chartData.ToCoinMarketChartDto();
                }
            }
            else
            {
                Console.WriteLine("[Get7DaysStatistics] Hata kodu 200 değil");
            }

            return null;
        }

        /// <summary>
        /// Belirli bir coinin son 15 günlük grafik verilerini döndürür.
        /// </summary>
        /// <param name="id">Coin ID (örnek: "ethereum")</param>
        /// <returns>15 günlük CoinMarketChartDto verisi</returns>
        public async Task<CoinMarketChartDto> Get15DaysStatistics(string id)
        {
            var result = await _httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency=usd&days=15");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var chartData = JsonConvert.DeserializeObject<CoinMarketChart>(content);
                if (chartData != null)
                {
                    return chartData.ToCoinMarketChartDto();
                }
            }
            else
            {
                Console.WriteLine("[Get15DaysStatistics] Hata kodu 200 değil");
            }

            return null;
        }

        /// <summary>
        /// Belirli bir coinin son 30 günlük grafik verilerini döndürür.
        /// </summary>
        /// <param name="id">Coin ID (örnek: "solana")</param>
        /// <returns>30 günlük CoinMarketChartDto verisi</returns>
        public async Task<CoinMarketChartDto> Get30DaysStatistics(string id)
        {
            var result = await _httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency=usd&days=30");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var chartData = JsonConvert.DeserializeObject<CoinMarketChart>(content);
                if (chartData != null)
                {
                    return chartData.ToCoinMarketChartDto();
                }
            }
            else
            {
                Console.WriteLine("[Get30DaysStatistics] Hata kodu 200 değil");
            }

            return null;
        }
    }
}
