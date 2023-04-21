using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Contract
    {
        public int Id { get; set; }
        public int AverageThroughput { get; set; }
        public int MaxCarCount { get; set; }
        public int RegularityOfDeliveries { get; set; }
        public string CompanyName { get; set; }
    }
}
