using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Collecting
    {
        public int ID { get; set; }
        public decimal TotalPaidFromMerchants { get; set; }
        public decimal TotalPaidForMerchants { get; set; }
        public decimal TotalHalek { get; set; }
        public decimal TotalForFahAllah { get; set; }
        public decimal TotalOfAdditionalPayment { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<AdditionalPayment> AdditionalPayments { get; set; }




    }
}
