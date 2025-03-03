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

        public CommentController( ICommentRepository commentRepo)
        {
            _commentRepo=commentRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id ){
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null){
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpPost]
        public async  Task<IActionResult> Create ([FromBody] CreateCommentRequestDto createCommentReq)
        {
            var commentModel = createCommentReq.toCommentFromCreateDto();
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new {id = commentModel.ToCommentDto()});
        }
      
    }
}