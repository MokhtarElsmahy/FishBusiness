using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class HalakaBuyReciept
    {
        [Key]
        public int HalakaBuyRecieptID { get; set; }

        [Display(Name = "تاريخ الفاتورة")]
        [Required(ErrorMessage = "برجاء ادخال تاريخ الفاتورة")]
        public DateTime Date { get; set; }


        [Display(Name = "الشخص البائع")]
        public string SellerName { get; set; }

        [Display(Name = "اجمالى السعر ")]
        public decimal TotalOfPrices { get; set; }

        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<HalakaBuyRecieptItem> HalakaBuyRecieptItems { get; set; }
    }
}
