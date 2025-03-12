using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string  AppUsersId { get; set; }
        public int StockId { get; set; }
        public AppUsers AppUser { get; set; }
        public Stock Stock { get; set; }
    }
}