using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comments
{
    public class CreateCommentRequestDto
    {
        // idsi olmicak 

        // Burada bir validate islem yapmamiz lazimsa
        
        public string Title { get; set; } = string.Empty;

        public string  Content { get; set; } = string.Empty;


    }
}