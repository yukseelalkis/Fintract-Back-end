using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comments
{
    public class UpdateCommentDto
    {
        
        public string Title { get; set; } = string.Empty;

        public string  Content { get; set; } = string.Empty;

        public DateTime CreateOn { get; set; } = DateTime.Now;


       
    }
}