using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SaleLogCreateViewModel
    {
        public List<Product> Products { get; set; }
        public List<Customer> Customers { get; set; }
        public SaleLog Data { get; set; }
    }
}
