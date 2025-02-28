using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api.Controllers
{
    [Route("api/stock")]
    public class StockControllers : ControllerBase
    {
        private readonly ApplicationDBContex _context;
        public StockControllers(ApplicationDBContex contex)
        {
            _context = contex;
        }

        [HttpGet]
        public IActionResult GetAll(){
            var stocks = _context.Stocks.ToList();

            return Ok(stocks);
        }


        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id ){

            var stock = _context.Stocks.Find(id);
            if(stock == null){
                return NotFound();
            }
            return Ok(stock);
        }
    }
}