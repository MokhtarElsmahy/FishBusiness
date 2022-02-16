using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;
using FishBusiness.ViewModels;

namespace FishBusiness.ViewModels
{
    public class SellerRecVm
    {
        public SellerRec SellerRec { get; set; }
        public List<SellerRecItem> NormalboatOwnerItems { get; set; }
        public List<SellerRecItem> AmountboatOwnerItems { get; set; }
        public List<AmountVm> Amounts { get; set; }
    }
}
