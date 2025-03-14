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
        public static  CommentDto ToCommentDto(this Comment commentModel){
                return new CommentDto{

                    Id = commentModel.Id,
                    Title = commentModel.Title,
                    Content = commentModel.Content,
                    CreateOn = commentModel.CreateOn,
                    CreatedBy = commentModel.Appuser.UserName,
                    StockId = commentModel.StockId
                };
        }
         public static  Comment ToCommentFromCreate(this CreateCommentRequestDto commentModel, int stockId){
                return new Comment{
                    Title = commentModel.Title,
                    Content = commentModel.Content,
                    StockId= stockId
                };
        }
        
         public static  Comment ToCommentUpdate(this UpdateCommentReqDto commentModel){
                return new Comment{
                    Title = commentModel.Title,
                    Content = commentModel.Content,
                };
        }
    }
}