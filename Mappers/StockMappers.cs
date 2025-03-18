using System;
using System.Collections.Generic;
using System.IO.Compression;
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
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(x => x.ToCommentDto()).ToList()

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

        public static Stock toStockFMP (this FMPStock fMPStock)
        {
            return new Stock{
                Symbol=fMPStock.symbol,
                CompanyName=fMPStock.companyName,
                Purchase = (decimal)fMPStock.price,
                LastDiv = fMPStock.lastDividend,
                Industry = fMPStock.industry,
                MarketCap = fMPStock.marketCap
            };
        }
    
    }
}