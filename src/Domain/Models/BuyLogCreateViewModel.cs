using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class BuyLogCreateViewModel
    {
        public List<Product> Products { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public BuyLog Data { get; set; }
    }
}
