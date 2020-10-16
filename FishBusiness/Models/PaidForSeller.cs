using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class PaidForSeller
    {
        public int ID { get; set; }
      

        [Display(Name = "المبلغ المدفوع")]
        public decimal Payment { get; set; }

        [Display(Name = "دين حتى اللحظة")]
        public decimal PreviousDebtsForSeller { get; set; }

        public DateTime Date { get; set; }

        public bool IsCash { get; set; }
        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }

        public int MerchantID { get; set; }

        public virtual Merchant Merchant { get; set; }
    }
}
