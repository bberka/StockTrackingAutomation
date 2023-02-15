using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class DebtLogCreateViewModel
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public DebtLog Data { get; set; }
    }
}
