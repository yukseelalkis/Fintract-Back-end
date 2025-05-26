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
    public class CoinService :ICoinService
    {
        private readonly HttpClient _httpClient;

        public CoinService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

        }

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
                        //return coinList.TOCoinModelDto();
                      return coinList.Select(x => x.ToCoinDto()).ToList();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Hata: {e.Message}");
            }
            return new List<CoinModelDto>();

        }

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
                        //return coinList.TOCoinModelDto();
                      return coinList.Select(x => x.ToCoinDto()).ToList();

                    }
                }
            }
            catch (Exception e)
            {
                
            Console.WriteLine($"Hata: {e.Message}");
            }
            
            return new List<CoinModelDto>();

        }

        public async Task<CoinMarketChartDto> Get7DaysStatistics(string id)
        {
            var result = await  _httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency=usd&days=7");
             if (result.IsSuccessStatusCode)
             {
                 var content = await result.Content.ReadAsStringAsync();
                var chartData = JsonConvert.DeserializeObject<CoinMarketChart>(content);
                if (chartData != null)
                {
                    return chartData.ToCoinMarketChartDto();
                }
             }
             else{
                Console.WriteLine("Hata kodu 200 degil");   
             }
            return null;
        }
         public async Task<CoinMarketChartDto> Get15DaysStatistics(string id)
        {
            var result = await  _httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency=usd&days=15");
             if (result.IsSuccessStatusCode)
             {
                 var content = await result.Content.ReadAsStringAsync();
                var chartData = JsonConvert.DeserializeObject<CoinMarketChart>(content);
                if (chartData != null)
                {
                    return chartData.ToCoinMarketChartDto();
                }
             }
             else{
                Console.WriteLine("Hata kodu 200 degil");   
             }
            return null;
        }
         public async Task<CoinMarketChartDto> Get30DaysStatistics(string id)
        {
            var result = await  _httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency=usd&days=30");
             if (result.IsSuccessStatusCode)
             {
                 var content = await result.Content.ReadAsStringAsync();
                var chartData = JsonConvert.DeserializeObject<CoinMarketChart>(content);
                if (chartData != null)
                {
                    return chartData.ToCoinMarketChartDto();
                }
             }
             else{
                Console.WriteLine("Hata kodu 200 degil");   
             }
            return null;
        }
    }

    
}
