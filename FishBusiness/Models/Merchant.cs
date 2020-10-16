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

        [Display(Name = "ديون على التاجر")]
        public decimal PreviousDebts { get; set; }
        [Display(Name = "تلفون التاجر")]
        [Required(ErrorMessage = "برجاء ادخال تلفون التاجر")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "عنوان التاجر")]
        public string Address { get; set; }

        [Display(Name = "ديون للتاجر")]
        public decimal PreviousDebtsForMerchant { get; set; }

        public bool IsFromOutsideCity { get; set; }
        public bool IsOwner { get; set; }


        public virtual ICollection<MerchantReciept> MerchantReciepts { get; set; }
        public virtual ICollection<IMerchantReciept> IMerchantReciepts { get; set; }
        public virtual ICollection<ISellerReciept> ISellerReciepts { get; set; }
        public virtual ICollection<SellerRec> SellerRecs { get; set; }
        public virtual ICollection<PaidForMerchant> PaidForMerchants { get; set; }

        public virtual ICollection<PaidForSeller> PaidForSellers { get; set; }
    }
}
