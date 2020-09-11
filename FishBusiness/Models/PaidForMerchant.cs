using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class PaidForMerchant
    {
        public int ID { get; set; }
        public int MerchantID { get; set; }

        [Display(Name = "المبلغ المدفوع")]
        public decimal Payment { get; set; }

        [Display(Name = "دين حتى اللحظة")]
        public decimal PreviousDebtsForMerchant { get; set; }

        public DateTime Date { get; set; }

        public bool IsPaidForUs { get; set; }


        public bool IsCash { get; set; }

        public virtual Merchant Merchant { get; set; }

    }
}
