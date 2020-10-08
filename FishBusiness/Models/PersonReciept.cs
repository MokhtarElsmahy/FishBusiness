using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class PersonReciept
    {
        public int PersonRecieptID { get; set; }

        [Display(Name = "اسم المشترى")]
        [Required(ErrorMessage = "برجاء ادخال اسم المشترى")]
        public string PersonName { get; set; }

        [Display(Name = "تاريخ الفاتورة")]
        [Required(ErrorMessage = "برجاء ادخال تاريخ الفاتورة")]
        public DateTime Date { get; set; }

        [Display(Name = "السعر")]
        [Required(ErrorMessage = "برجاء ادخال سعر الفاتورة")]
        public decimal TotalPrice { get; set; }

        public virtual ICollection<PersonRecieptItem> PersonRecieptItem { get; set; }
    }
}
