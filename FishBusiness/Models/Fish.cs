using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Fish
    {
        [Key]
        public int FishID { get; set; }

        [Display(Name ="الاسم")]
        [Required(ErrorMessage ="برجاء ادخال الاسم")]

        public string FishName { get; set; }

        public virtual ICollection<BoatOwnerItem> BoatOwnerItems { get; set; }
        public virtual ICollection<MerchantRecieptItem> MerchantRecieptItems { get; set; }
        public virtual ICollection<PersonRecieptItem> PersonRecieptItems { get; set; }
        public virtual ICollection<ISellerRecieptItem> ISellerRecieptItems { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }

    }
}
