using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class ProductionType
    {
        [Key]
        public int ProductionTypeID { get; set; }
        [Required(ErrorMessage = "برجاء ادخال الانتاج ")]

        //طوايل ---ميزان
        public string ProductionName { get; set; }

        public virtual ICollection<BoatOwnerItem> BoatOwnerItems { get; set; }
        public virtual ICollection<MerchantRecieptItem> MerchantRecieptItems { get; set; }

    }
}
