using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TypeTerminal : EntityBase
    {
        public string Name { get; set; }
        public int MinGates { get; set; }
        public int MaxGates { get; set; }
        public string FormTerminal { get; set; }
    }
}
