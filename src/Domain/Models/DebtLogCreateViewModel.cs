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
        public List<Customer> Customers { get; set; }
        public DebtLog Data { get; set; }
    }
}
