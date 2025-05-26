using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    /// <summary>
    /// Yorum işlemlerini yöneten API controller sınıfıdır.
    /// CRUD işlemleri, kullanıcı doğrulaması ve hisse senedi eşleşmeleri yapılır.
    /// </summary>
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUsers> _userManager;
        private readonly IFMPService _fmpService;

        public CommentController(
            ICommentRepository commentRepo,
            IStockRepository stockRepo,
            UserManager<AppUsers> userManager,
            IFMPService fmpService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        /// <summary>
        /// Mevcut tüm yorumları filtreleyerek getirir.
        /// </summary>
        /// <param name="queryObject">Filtreleme ve sıralama parametreleri.</param>
        /// <returns>Yorum listesi.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllAsync(queryObject);
            var commentDto = comments.Select(s => s.ToCommentDto());

            return Ok(commentDto);
        }

        /// <summary>
        /// Belirli bir yorumun detaylarını getirir.
        /// </summary>
        /// <param name="id">Yorumun ID değeri.</param>
        /// <returns>Yorum detay bilgisi.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDto());
        }

        /// <summary>
        /// Yeni bir yorum oluşturur. Hisse senedi bulunamazsa FMP servisinden alınır.
        /// </summary>
        /// <param name="symbol">Yorum yapılacak hisse senedinin sembolü.</param>
        /// <param name="createCommentReq">Yorum bilgilerini içeren DTO.</param>
        /// <returns>Oluşturulan yorumun detayları.</returns>
        [HttpPost]
        [Route("{symbol:alpha}")]
        public async Task<IActionResult> Create([FromRoute] string symbol, [FromBody] CreateCommentRequestDto createCommentReq)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepo.GetBySymbol(symbol);

            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                    return BadRequest("Stock is not exist");
                else
                    await _stockRepo.CreateAsync(stock);
            }

            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);

            var commentModel = createCommentReq.ToCommentFromCreate(stock.Id);
            commentModel.AppUserId = appUser.Id;

            await _commentRepo.CreatedAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        /// <summary>
        /// Mevcut bir yorumu günceller.
        /// </summary>
        /// <param name="id">Yorumun ID değeri.</param>
        /// <param name="updateDto">Güncellenecek verileri içeren DTO.</param>
        /// <returns>Güncellenmiş yorum bilgisi.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentReqDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentUpdate());

            if (comment == null)
                return NotFound("Comment not found");

            return Ok(comment.ToCommentDto());
        }

        /// <summary>
        /// Belirli bir yorumu siler.
        /// </summary>
        /// <param name="id">Silinecek yorumun ID değeri.</param>
        /// <returns>Silinen yorum bilgisi.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var commentModel = await _commentRepo.DeleteAsync(id);

            if (commentModel == null)
                return NotFound("Comment does not exist");

            return Ok(commentModel);
        }
    }
}
