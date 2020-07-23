using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Comission
    {
        [Key]
        public int CommisionID { get; set; }

        [Required(ErrorMessage = "ادخل قيمة العموله")]
        [Display(Name = "العموله")]
        public decimal CommisionValue { get; set; }

        [Required(ErrorMessage = "ادخل التاريخ")]
        [Display(Name = "التاريخ")]
        public DateTime Date { get; set; }
    }
}
