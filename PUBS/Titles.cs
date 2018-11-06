using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUBS
{
    class Titles
    {
        public Titles(string Title, string Type, string Price, string Advance, string Royalty, string Sales, string Date)
        {
            this.Title = Title;
            this.Type = Type;
            this.Price = Price;
            this.Advance = Advance;
            this.Royalty = Royalty;
            this.Sales = Sales;
            this.Date = Date;
        }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
        public string Advance { get; set; }
        public string Royalty { get; set; }
        public string Sales { get; set; }
        public string Date { get; set; }
    }
}
