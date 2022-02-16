using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class MasterReciept
    {
        // الجدول ده محطوط فيه تجميعة الفواتير اللى متحاسب عليهم فى نفس اليوم 
        [Key]
        public int MasterRecieptID { get; set; }

     
        public int? BoatID { get; set; }



       
        public int? SarhaID { get; set; }
      


        [Display(Name = "انتاج السرحة")]
        public decimal TotalBeforePaying { get; set; }

        [Required(ErrorMessage = "برجاء ادخال تاريخ الفاتورة")]
        [Display(Name = "تاريخ الفاتوره")]
        public DateTime Date { get; set; }

        [Display(Name = "العموله")]
        public decimal Commission { get; set; }

        [Display(Name = "نسبة العموله")]
        public int PercentageCommission { get; set; }

        [Required(ErrorMessage = "برجاء ادخال المدفوع من الهالك")]
        [Display(Name = "المدفوع من الهالك")]
        public decimal PaidFromDebts { get; set; }

        [Display(Name = "صافى انتاج السرحة")]
        public decimal TotalAfterPaying { get; set; }


        [Display(Name = "ايراد السرحة")]
        public decimal FinalIncome { get; set; }

        public bool IsCalculated { get; set; }
        public bool IsCollected { get; set; }
        public bool IsCheckedOut { get; set; }

        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }

       // public virtual ICollection<BoatOwnerItem> BoatOwnerItems { get; set; }
    }
}
