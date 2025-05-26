using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.models;

namespace api.Mappers
{
    public static class coinMappers
    {
      public static CoinModelDto ToCoinDto(this CoinModel coinModel)
{
    return new CoinModelDto
    {
        Id = coinModel.Id,
        Symbol = coinModel.Symbol,
        Name = coinModel.Name,
        Image = coinModel.Image,
        Current_Price = coinModel.Current_Price,
        Market_Cap = coinModel.Market_Cap,
        Market_Cap_Rank = coinModel.Market_Cap_Rank,
        Fully_Diluted_Valuation = coinModel.Fully_Diluted_Valuation,
        Total_Volume = coinModel.Total_Volume,
        High_24h = coinModel.High_24h,
        Low_24h = coinModel.Low_24h,
        Price_Change_24h = coinModel.Price_Change_24h,
        Price_Change_Percentage_24h = coinModel.Price_Change_Percentage_24h,
        Market_Cap_Change_24h = coinModel.Market_Cap_Change_24h,
        Market_Cap_Change_Percentage_24h = coinModel.Market_Cap_Change_Percentage_24h,
        Circulating_Supply = coinModel.Circulating_Supply,
        Total_Supply = coinModel.Total_Supply,
        Max_Supply = coinModel.Max_Supply,
        Ath = coinModel.Ath,
        Ath_Change_Percentage = coinModel.Ath_Change_Percentage,
        Ath_Date = coinModel.Ath_Date,
        Atl = coinModel.Atl,
        Atl_Change_Percentage = coinModel.Atl_Change_Percentage,
        Atl_Date = coinModel.Atl_Date,
        Roi = coinModel.Roi,
        Last_Updated = coinModel.Last_Updated
    };
}

    }
}