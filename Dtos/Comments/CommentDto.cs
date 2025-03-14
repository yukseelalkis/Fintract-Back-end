using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string  Content { get; set; } = string.Empty;

        public DateTime CreateOn { get; set; } = DateTime.Now;
        // kim tarafindan yapidligii bakmak icin yaptik
        // mapperdada dgisecek
        public string  CreatedBy { get; set; } = string.Empty;

        // FK OLDUGUNDAN DIKKAT ETMELIYIZ 
        public int? StockId { get; set; }

    }
}