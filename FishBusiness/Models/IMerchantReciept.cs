using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class IMerchantReciept
    {
        [Key]
        public int IMerchantRecieptID { get; set; }

        [Display(Name = "تاريخ الفاتورة")]
        [Required(ErrorMessage = "برجاء ادخال تاريخ الفاتورة")]
        public DateTime Date { get; set; }
        [ForeignKey("Merchant")]
        public int MerchantID { get; set; }
        public virtual Merchant Merchant { get; set; }

        [Display(Name = "اجمالى الفاتورة")]
        public decimal TotalOfReciept { get; set; }



        public virtual ICollection<IMerchantRecieptItem> IMerchantRecieptItems { get; set; }
        
    }
}
