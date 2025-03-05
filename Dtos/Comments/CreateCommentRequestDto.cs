using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comments
{
    public class CreateCommentRequestDto
    {
        // idsi olmicak 

        // BUrada yorumun en az 5 karekter icermesini istiyorsak DOGRULAMA ISLEMI ICIN GEREKLI
        // Dogrulama mesajlari cok tekrara dusuyorr buna bakilacak!!!!!!!!!!
        [Required]
        [MinLength(5,ErrorMessage ="Title  must  be 5 characters")]
        [MaxLength(280 , ErrorMessage ="Title cannot be over  280 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5,ErrorMessage ="Content  must  be 5 characters")]
        [MaxLength(280 , ErrorMessage ="Title cannot be over  280 characters")]

        public string  Content { get; set; } = string.Empty;


    }
}