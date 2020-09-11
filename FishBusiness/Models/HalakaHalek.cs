using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class HalakaHalek
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Debt")]
        public int DebtID { get; set; }

        public virtual Debt Debt { get; set; }


    }
}
