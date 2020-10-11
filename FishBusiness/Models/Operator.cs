using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Operator
    {
        public int OperatorID { get; set; }
        [Display(Name = "اسم العميل")]
        [Required(ErrorMessage = "برجاء ادخال اسم العميل")]
        public string OperatorName { get; set; }

        [Display(Name = "ديون للعميل")]
        public decimal Credit { get; set; }
        [Display(Name = "تلفون العميل")]
        [Required(ErrorMessage = "برجاء ادخال تلفون العميل")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "عنوان العميل")]
        public string Address { get; set; }
        [Display(Name = "وصف العمل")]
        public string JobDesc { get; set; }

    }
}
