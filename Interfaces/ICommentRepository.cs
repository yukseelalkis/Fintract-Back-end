using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id ); 
        Task<Comment> CreateAsync(Stock stockModel);
       // Task<Comment> UpdateAsync(int id , UpdateStockRequestDto updateStockRequestDto);
        Task<Comment> DeleteAsync(int id );
    }
}