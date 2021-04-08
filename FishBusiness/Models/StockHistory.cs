using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class StockHistory
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Fish")]
        public int FishID { get; set; }
        public Fish Fish { get; set; }


        [ForeignKey("ProductionType")]
        public int ProductionTypeID { get; set; }
        public ProductionType ProductionType { get; set; }

        public double Total { get; set; }
    }
}
