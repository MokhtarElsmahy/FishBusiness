using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class HalakSellReciept
    {
        [Key]
        public int HalakSellRecieptID { get; set; }

        [Display(Name = "تاريخ الفاتورة")]
        [Required(ErrorMessage = "برجاء ادخال تاريخ الفاتورة")]
        public DateTime Date { get; set; }


        [Display(Name = "الشخص المشترى")]
        public string  buyerName { get; set; }

        [Display(Name = "اجمالى السعر ")]
        public decimal TotalOfPrices { get; set; }

        public bool IsCash { get; set; } //يعنى تم دفعها فى نفس الوقت ولا لسه هتتدفع بكره

        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<HalakSellRecieptItem> ISellerRecieptItems { get; set; }
    }
}
