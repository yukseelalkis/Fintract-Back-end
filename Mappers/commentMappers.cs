using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.models;

namespace api.Mappers
{
    public static class commentMappers
    {
        public static CommentDto ToCommentDto(this Comment commentModel){
                return new CommentDto{

                    Id = commentModel.Id,
                    Title = commentModel.Title,
                    Content = commentModel.Content,
                    CreateOn = commentModel.CreateOn,
                    StockId = commentModel.StockId
                };
        }

       public static Comment toCommentFromCreateDto(this CreateCommentRequestDto createCommentReq)
       {
        return new Comment{
            Title = createCommentReq.Title,
            Content = createCommentReq.Content,
            CreateOn = createCommentReq.CreateOn,
            StockId = createCommentReq.StockId
        };
       }
       public static Comment toCommentFromUpdateDto(this UpdateCommentDto updateComment){
        return new Comment{
            Title = updateComment.Title,
            Content = updateComment.Content,
            CreateOn = updateComment.CreateOn,
        };
       }
    }
}