using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Cofig
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="الاسم")]
        [Required(ErrorMessage ="برجاء ادخال الاعدادات")]
        public string Name { get; set; }


        [Display(Name = "القيمة")]
        [Required(ErrorMessage = "برجاء ادخال القيمة")]
        public string Value { get; set; }
    }
}
