using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LocModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Lang { get; set; }
        public int ParamCount { get; set; }
    }
}
