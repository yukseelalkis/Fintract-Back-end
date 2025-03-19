using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comments;
using api.Helpers;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContex _context;
        public CommentRepository(ApplicationDBContex context)
        {
            _context=context;
        }
        ////////////////////// CREATET METOTT  /////////////////// /////
        public async Task<Comment> CreatedAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }
        ///////////////////// Delete  ////////////////////////////////////////
        public async Task<Comment?> DeleteAsync(int id)
        {
           var commentModel = await _context.Comments.FirstOrDefaultAsync(x=> x.Id == id );
           if (commentModel == null )
           {
             return null ;
           }        
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        ///////////////////////// GETLERIMIZ ////////////////////////////////
        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
             var comments =   _context.Comments.Include(a=>a.Appuser).AsQueryable();
             if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
             {
                comments = comments.Where(s=> s.Stock.Symbol == queryObject.Symbol);
             }
             if (queryObject.IsDecsending == true)
             {
                comments = comments.OrderByDescending(c=> c.CreateOn);
             }
             return await comments.ToListAsync();
        }
        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(a=>a.Appuser).FirstOrDefaultAsync(c=>c.Id == id);
        }
        ///////////////////// Update Metottt  ////////////////////////////////////////
        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var exitingComment = await  _context.Comments.FindAsync(id);
                if(exitingComment == null){
                    return null;
                }
                // bos degilse guncelleme islemlerini yapilacak 
                exitingComment.Title = commentModel.Title;
                exitingComment.Content= commentModel.Content;
                await _context.SaveChangesAsync();
                return exitingComment;
        }


    }
}