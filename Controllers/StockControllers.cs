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

namespace api.Controllers
{
    /// <summary>
    /// Hisse senedi işlemlerini yöneten API controller'dır.
    /// CRUD işlemleri ve popüler hisse verileri sağlanır.
    /// </summary>
    [Route("api/stock")]
    [ApiController]
    public class StockControllers : ControllerBase
    {
        private readonly ApplicationDBContex _context;
        private readonly IStockRepository _stockRepository;

        public StockControllers(ApplicationDBContex contex, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            _context = contex;
        }

        /// <summary>
        /// Tüm hisse senedi listesini döndürür (filtrelenebilir).
        /// </summary>
        /// <param name="query">Sayfalama, arama ve sıralama için sorgu nesnesi.</param>
        /// <returns>Hisse senedi DTO listesi.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockRepository.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

            return Ok(stockDto);
        }

        /// <summary>
        /// Belirli bir hisse senedini ID ile getirir.
        /// </summary>
        /// <param name="id">Hisse senedinin ID değeri.</param>
        /// <returns>Hisse DTO nesnesi.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
                return NotFound();

            return Ok(stock.ToStockDto());
        }

        /// <summary>
        /// Popüler hisse senetlerini getirir.
        /// </summary>
        /// <param name="count">Dönmek istenen popüler hisse sayısı. (Varsayılan: 5)</param>
        /// <returns>Popüler hisse DTO listesi.</returns>
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularStocks([FromQuery] int count = 5)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockRepository.GetPopularStocksAsync(count);
            var stockDtos = stocks.Select(s => s.ToStockDto()).ToList();

            return Ok(stockDtos);
        }

        /// <summary>
        /// Yeni bir hisse senedi kaydı oluşturur.
        /// </summary>
        /// <param name="stockDto">Yeni hisse verilerini içeren DTO.</param>
        /// <returns>Oluşturulan hisse DTO'su ve 201 cevabı.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = stockDto.toStockFromCreateDto();
            await _stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        /// <summary>
        /// Var olan bir hisse senedi kaydını günceller.
        /// </summary>
        /// <param name="id">Güncellenecek hisse ID'si.</param>
        /// <param name="updateDto">Güncelleme verilerini içeren DTO.</param>
        /// <returns>Güncellenmiş hisse DTO nesnesi.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepository.UpdateAsync(id, updateDto);
            if (stockModel == null)
                return NotFound();

            return Ok(stockModel.ToStockDto());
        }

        /// <summary>
        /// Belirli bir hisse senedi kaydını siler.
        /// </summary>
        /// <param name="id">Silinecek hisse senedinin ID'si.</param>
        /// <returns>NoContent(204) yanıtı döner.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepository.DeleteAsync(id);
            if (stockModel == null)
                return NotFound();

            return NoContent();
        }
    }
}
