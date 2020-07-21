using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Merchant
    {
        public int MerchantID { get; set; }
        [Display(Name = "اسم التاجر")]
        [Required(ErrorMessage = "برجاء ادخال اسم التاجر")]
        public string MerchantName { get; set; }

        [Display(Name = "الديون السابقة")]
        public decimal PreviousDebts { get; set; }
        [Display(Name = "تلفون التاجر")]
        [Required(ErrorMessage = "برجاء ادخال تلفون التاجر")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "عنوان التاجر")]
        public string Address { get; set; }

        public virtual ICollection<MerchantReciept> MerchantReciepts { get; set; }
    }
}
