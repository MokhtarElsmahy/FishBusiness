using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class MerchantReciept
    {
        [Key]
        public int MerchantRecieptID { get; set; }

        [Display(Name = "تاريخ الفاتورة")]
        [Required(ErrorMessage = "برجاء ادخال تاريخ الفاتورة")]
        public DateTime Date { get; set; }
        [ForeignKey("Merchant")]
        public int MerchantID { get; set; }
        public virtual Merchant Merchant { get; set; }

        [Display(Name = "اجمالى الفاتورة")]
        public decimal TotalOfReciept { get; set; }

        [Display(Name = "ما تم دفعه")]
        [Required(ErrorMessage = "برجاء ادخال المبلغ")]
        public decimal payment { get; set; }


        [Display(Name = "ديون حتى اللحظه")]
     
        public decimal CurrentDebt { get; set; }

        public virtual ICollection<MerchantRecieptItem> MerchantRecieptItems { get; set; }

        public int? FromMerchant { get; set; }//التاجر اللى تم الشراء منه 
        //بيتم تسجيله فى 
        //SellerRecsController -- Mcreat()
        //علشان نحدد التاجر اشترى من مين 
        //


        //[Display(Name = "ما تم دفعه")]

        //public decimal CurrentDebts { get; set; }
    }
}
