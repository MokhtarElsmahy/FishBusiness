using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;

namespace FishBusiness.ViewModels
{
    public class IsellerLatestRecForMerchantVm
    {
        public  ISellerReciept sellerReciept { get; set; }
        public  List<ISellerRecieptItem> sellerRecieptItems { get; set; }
    }
}
