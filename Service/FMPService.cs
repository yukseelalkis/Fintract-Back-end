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
    // api key== z2tJqbcRLX9q43biJwuaI08AdScABoyX
    public class FMPService : IFMPService
    {
        // ​HttpClient, .NET'te HTTP istekleri göndermek ve yanıtları almak için kullanılan bir sınıftır. ​
        private HttpClient _httpClient;
        //IConfiguration, uygulama yapılandırma ayarlarına erişim sağlayan bir arayüzdür. ​
        private IConfiguration _config;
        public FMPService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient=httpClient;
            _config=config;
        }
        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
           try
           {
            //tr
            //var result = await  _httpClient.GetAsync($"https://financialmodelingprep.com/stable/profile/{symbol}?&apikey={_config["FMPKey"]}");
            // amerika
            var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");
            //https://financialmodelingprep.com/api/v3/profile/MA?apikey=z2tJqbcRLX9q43biJwuaI08AdScABoyX
            if (result.IsSuccessStatusCode)
            {
                var content = await  result.Content.ReadAsStringAsync();
                // dto/stock/FMPStock
                var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                var stock  = tasks[0];
                if (stock != null )
                {
                    // mappersta ayalamlarimizi yaptik
                    return stock.toStockFMP();
                }
                return null;
            } 
           }
           catch (Exception e) 
           {
                Console.WriteLine(e);
                return null;
           }
           return null;
        }
    }
}