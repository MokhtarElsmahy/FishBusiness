using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class StockRec
    {
        public int StockRecID { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalOfRec { get; set; }


        public virtual ICollection<StockRecItem> StockRecItems { get; set; }
    }
}
