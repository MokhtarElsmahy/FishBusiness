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
        public virtual ICollection<IMerchantRecieptItem> IMerchantRecieptItems { get; set; }
        public virtual ICollection<PersonRecieptItem> PersonRecieptItems { get; set; }
        public virtual ICollection<ISellerRecieptItem> ISellerRecieptItems { get; set; }
        public virtual ICollection<StockRecItem> StockRecItems { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }

        public virtual ICollection<SellerRecItem> SellerRecItems { get; set; }

    }
}
