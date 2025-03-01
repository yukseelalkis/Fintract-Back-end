using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDtos  ToStockDto(this Stock  stockModel){

            return new StockDtos{
                Id = stockModel.Id,
                Symbol=stockModel.Symbol,
                CompanyName=stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap

            };
        }
        public static Stock toStockFromCreateDto (this CreateStockRequestDto createStockRequestDto)
        {
            return new Stock{
                Symbol=createStockRequestDto.Symbol,
                CompanyName=createStockRequestDto.CompanyName,
                Purchase = createStockRequestDto.Purchase,
                LastDiv = createStockRequestDto.LastDiv,
                Industry = createStockRequestDto.Industry,
                MarketCap = createStockRequestDto.MarketCap
            };
        }
    
    }
}