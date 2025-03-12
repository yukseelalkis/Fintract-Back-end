using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]



    public class StockControllers : ControllerBase
    {
        private readonly ApplicationDBContex _context;
        private readonly IStockRepository _stockRepository;

        public StockControllers(ApplicationDBContex contex , IStockRepository stockRepository )
        {
            _stockRepository = stockRepository;
            _context = contex;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetAll(){
        //     // bu oncesi dtosuz neden diye gptye sorduk cevaplar 
        //     //Bu değişikliğin sebebi, veritabanı modelini dış dünyaya (API tüketicisine) doğrudan açmamak ve daha güvenli, optimize bir veri yapısı sunmaktır.
        //     //2️⃣ Verinin Optimize Edilmesi (Az Veri Göndermek)
        //     //3️⃣ ORM (Entity Framework) Bağlılık Problemlerini Önlemek
        //     //4️⃣ API’nın Dışarıya Olan Bağımlılığını Azaltmak    
        //    // var stocks = await  _context.Stocks.ToListAsync();
        //    // comment ile farki reposorty icinde gosteriyor o da incluede metodu
        //       if (!ModelState.IsValid)
        //         return BadRequest(ModelState);
        //    var stocks = await  _stockRepository.GetAllAsync();
        //     var stockDto = stocks.Select(s=> s.ToStockDto());
        //     return Ok(stocks);
        // }
        [HttpGet]
         public async Task<IActionResult> GetAll([FromQuery] QueryObject query){
            if (!ModelState.IsValid)
                 return BadRequest(ModelState);
            var stocks = await  _stockRepository.GetAllAsync(query);
             var stockDto = stocks.Select(s=> s.ToStockDto());
             return Ok(stocks);
         }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id ){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock = await _stockRepository.GetByIdAsync(id );
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create ([FromBody] CreateStockRequestDto stockDto){
             if (!ModelState.IsValid)
                return BadRequest(ModelState);           
             var stockModel = stockDto.toStockFromCreateDto();
                await _stockRepository.CreateAsync(stockModel);
             return  CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update ([FromRoute] int id ,[FromBody] UpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            
            var stockModel = await _stockRepository.UpdateAsync(id,updateDto );
            if (stockModel == null)
            {
                return NotFound();
            }
            return Ok(stockModel.ToStockDto());
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete  ([FromRoute] int id )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = await _stockRepository.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }
        
            return NoContent();

        }

    }
}