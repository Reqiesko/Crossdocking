using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Terminal
    {
        public int Id { get; set; }
        public int CountGates { get; set; }
        public int CountLoader { get; set; }
        public int CountProduct { get; set; }
        public int CountBelt { get; set; }
    }
}
