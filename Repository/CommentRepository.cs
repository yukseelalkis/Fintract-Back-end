using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comments;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        // Bunlarin dahah iyi elbette yazilir bu durumda generic yapilar kullanilark yazilabilir 
        // zaten su anda kod tekrari fazla olmasindan dolayi sikintili oldugunu dusunuyorrum 
        private readonly ApplicationDBContex _context;
        public CommentRepository(ApplicationDBContex context)
        {
            _context=context;
        }
        public async Task<Comment> CreateAsync(Comment commentModel)
        {
          await _context.Comments.AddAsync(commentModel);
          await _context.SaveChangesAsync();
           return commentModel;
        }

        public async Task<Comment> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(x=>x.Id == id);
               if (commentModel == null)
            {
                return null;
            }
            _context.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await  _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment> UpdateAsync(int id, UpdateCommentDto updateComment)
        {
            var exitingComment = await _context.Comments.FirstOrDefaultAsync(x=>x.Id == id);
               if (exitingComment == null){
                return null;}
            exitingComment.Title = updateComment.Title;
            exitingComment.Content = updateComment.Content;
            exitingComment.CreateOn = updateComment.CreateOn;
            await  _context.SaveChangesAsync();
            return exitingComment;
        }
    }
}