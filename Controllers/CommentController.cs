using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        
        private readonly ICommentRepository _commentRepo;
        // stock repo uretmemeizin sebebi stock reponun  stockidsine ulasmamiz lazim 
        private readonly IStockRepository _stockRepo;

        public CommentController( ICommentRepository commentRepo , IStockRepository  stockRepo)
        {
            _commentRepo=commentRepo;
            _stockRepo=stockRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id ){
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null){
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
        /// CREATEED METOT
        [HttpPost("{stockId:int}")]
        public async  Task<IActionResult> Create ([FromRoute] int stockId,[FromBody] CreateCommentRequestDto createCommentReq)
        {
            // iste stock id Kontrol ediyoruz boyle bir stock var mi dye bakiyoruz yoksa hata veriyoruz

            if (!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock is not exist!!!");
            }
             var commentModel = createCommentReq.ToCommentFromCreate(stockId);
             await _commentRepo.CreatedAsync(commentModel);
             return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());
        }
      /// Update put metot 
      [HttpPut]
      [Route("{id:int}")]
      public async Task<IActionResult> Update ([FromRoute] int id ,[FromBody] UpdateCommentReqDto updateDto){
        var comment  = await _commentRepo.UpdateAsync(id,updateDto.ToCommentUpdate());
        if(comment == null )
        {
            return NotFound("Comment not Found ");
        }
        return Ok(comment.ToCommentDto());
      }

      [HttpDelete]
      [Route("{id:int}")]
      public  async Task<IActionResult> Delete ([FromRoute]int id ){
        var commentModel = await  _commentRepo.DeleteAsync(id);
        if(commentModel == null )
        {
            return NotFound("Comment does not exist!!");
        }
        return Ok(commentModel);
    }
 }
}