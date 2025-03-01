using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Controllers
{
    [Route("api/stock")]
    public class StockControllers : ControllerBase
    {
        private readonly ApplicationDBContex _context;
        private readonly IStockRepository _stockRepository;

        public StockControllers(ApplicationDBContex contex , IStockRepository stockRepository )
        {
            _stockRepository = stockRepository;
            _context = contex;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            // bu oncesi dtosuz neden diye gptye sorduk cevaplar 
            //Bu değişikliğin sebebi, veritabanı modelini dış dünyaya (API tüketicisine) doğrudan açmamak ve daha güvenli, optimize bir veri yapısı sunmaktır.
            //2️⃣ Verinin Optimize Edilmesi (Az Veri Göndermek)
            //3️⃣ ORM (Entity Framework) Bağlılık Problemlerini Önlemek
            //4️⃣ API’nın Dışarıya Olan Bağımlılığını Azaltmak    
           // var stocks = await  _context.Stocks.ToListAsync();
           var stocks = await  _stockRepository.GetAllAsync();
            var stockDto = stocks.Select(s=> s.ToStockDto());
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id ){

            var stock = await _stockRepository.GetByIdAsync(id );
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create ([FromBody] CreateStockRequestDto stockDto){
             var stockModel = stockDto.toStockFromCreateDto();
                await _stockRepository.CreateAsync(stockModel);
             return  CreatedAtAction(nameof(GetById), new {id = stockModel.ToStockDto()});
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update ([FromRoute] int id ,[FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _stockRepository.UpdateAsync(id,updateDto );
            if (stockModel == null)
            {
                return NotFound();
            }
            return Ok(stockModel.ToStockDto());
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete  ([FromRoute] int id )
        {
            var stockModel = await _stockRepository.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }
        
            return NoContent();

        }

    }
}