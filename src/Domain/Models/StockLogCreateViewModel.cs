using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StockLogCreateViewModel
    {
        public List<Product> Products { get; set; }
        public StockLog Data { get; set; }
    }
}
