using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class SellerRec
    {
        [Key]
        public int SellerRecID { get; set; }

        [ForeignKey("Merchant")]
        public int MerchantID { get; set; }

        public virtual Merchant Merchant { get; set; }

        [Display(Name = "انتاج السرحة")]
        public decimal TotalBeforePaying { get; set; }

        [Required(ErrorMessage = "برجاء ادخال تاريخ الفاتورة")]
        [Display(Name = "تاريخ الفاتوره")]
        public DateTime Date { get; set; }

        [Display(Name = "العموله")]
        public decimal Commission { get; set; }

        [Display(Name = "نسبة العموله")]
        public int PercentageCommission { get; set; }

        [Display(Name = "ايراد السرحة")]
        public decimal FinalIncome { get; set; }

        public virtual ICollection<SellerRecItem> SellerRecItems { get; set; }
    }
}
