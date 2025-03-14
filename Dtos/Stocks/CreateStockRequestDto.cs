using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stocks
{
    public class CreateStockRequestDto
    {
        //Symbol
        [Required]
        [MaxLength (10, ErrorMessage ="Symbol cannot  ve over 10 characters")]   
        [MinLength (2, ErrorMessage ="Symbol cannot  ve min 2 characters")]   
        public string  Symbol { get; set; } = string.Empty;
        //CompanyName
        [Required]
        [MaxLength (20, ErrorMessage ="CompanyName cannot  ve over 10 characters")]   
        public string  CompanyName { get; set; } = string.Empty;
        //Purchase
        [Required]
        [Range(1,100000)]
        public decimal  Purchase { get; set; }
        //LastDiv
        [Required]
        [Range(0.001,100)]
        public decimal LastDiv { get; set; }
        //Industry
        [Required]
        [MaxLength (10, ErrorMessage ="Industry cannot  ve over 10 characters")]   
        public string Industry { get; set; } = string.Empty;
        //MarketCap
        [Range(1,500000)]
        public long  MarketCap { get; set; }

    }
}