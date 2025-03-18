using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        
        /// degisikliler yuapiyoruzz zart zurt 
        private readonly ICommentRepository _commentRepo;
        // stock repo uretmemeizin sebebi stock reponun  stockidsine ulasmamiz lazim 
        private readonly IStockRepository _stockRepo;

        private readonly UserManager<AppUsers> _userManager;
        private readonly IFMPService _fmpService;

        public CommentController( ICommentRepository commentRepo , IStockRepository  stockRepo,
        UserManager<AppUsers> userManager, IFMPService fMPService)
        {
            _commentRepo=commentRepo;
            _stockRepo=stockRepo;
            _userManager = userManager;
            _fmpService=fMPService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
             //ModelState, gelen HTTP isteğindeki verilerin model kurallarına uyup uymadığını kontrol eden bir nesnedir.
             if (!ModelState.IsValid)
                 return BadRequest(ModelState);
             var comments = await _commentRepo.GetAllAsync();
             var commentDto = comments.Select(s => s.ToCommentDto());
             return Ok(commentDto);
         }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id ){
            /// daha az tekrarli yazilabilir
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null){
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
        /// CREATEED METOT
        [HttpPost]
        [Route("{symbol:alpha}")]
        public async  Task<IActionResult> Create ([FromRoute] string  symbol,[FromBody] CreateCommentRequestDto createCommentReq)
        {
            // iste stock id Kontrol ediyoruz boyle bir stock var mi dye bakiyoruz yoksa hata veriyoruz
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           // symbol var mi kontrol saglamasi  async yazmamisiz ama cagirdigimiz fonksiyon bir asyncdir.  
            var stock = await   _stockRepo.GetBySymbol(symbol);
            // yoksa  ne dondurmesi gerekiyor onu yazacaz
            if(stock == null){
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock is not exist");
                }
                else{
                    await _stockRepo.CreateAsync(stock);
                }
            }
            var userName = User.GetUserName();
            var AppUser = await _userManager.FindByNameAsync(userName);

            var commentModel = createCommentReq.ToCommentFromCreate(stock.Id);

            commentModel.AppUserId =AppUser.Id;

            await _commentRepo.CreatedAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());
        }
      /// Update put metot 
      [HttpPut]
      [Route("{id:int}")]
      public async Task<IActionResult> Update ([FromRoute] int id ,[FromBody] UpdateCommentReqDto updateDto){
        /// her yerde bunu yazmamiz sacmaaaaaaaaaaaaaaaaaaaaaaaaaaa
        /// 
           if (!ModelState.IsValid)
                return BadRequest(ModelState);
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