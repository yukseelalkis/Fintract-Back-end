using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Helpers;
using api.models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        
        Task<List<Comment>> GetAllAsync(CommentQueryObject commentQueryObject);
        Task<Comment?> GetByIdAsync(int id ); 
        Task<Comment> CreatedAsync(Comment commentModel);
        Task<Comment?> UpdateAsync(int id , Comment comment);
        Task<Comment?> DeleteAsync(int id );
    }
}