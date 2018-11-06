using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUBS
{
    class Sales
    {
        public Sales(string Date, string Quantity)
        {
            this.Date = Date;
            this.Quantity = Quantity;
        }
        public string Date { get; set; }
        public string Quantity { get; set; }
    }
}
