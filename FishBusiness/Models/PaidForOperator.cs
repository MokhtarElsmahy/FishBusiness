using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class PaidForOperator
    {
        public int ID { get; set; }
        public int OperatorID { get; set; }

        [Display(Name = "المبلغ المدفوع")]
        public decimal Payment { get; set; }

        [Display(Name = "دين حتى اللحظة")]
        public decimal DebtsAfterPayment { get; set; }

        public DateTime Date { get; set; }
        [ForeignKey("Person")]
        public int PersonID { get; set; }

        public virtual Operator Operator { get; set; }
        public virtual Person Person { get; set; }

    }
}
