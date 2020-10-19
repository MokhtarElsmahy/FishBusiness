using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class PaidForBoat
    {
        public int ID { get; set; }


        [Display(Name = "المبلغ المدفوع")]
        public decimal Payment { get; set; }

        [Display(Name = "دين حتى اللحظة")]
        public decimal HalekDebtsTillNow { get; set; }

        public DateTime Date { get; set; }
        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }

        public int BoatID { get; set; }

        public virtual Boat Boat { get; set; }
    }
}
