using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;
namespace FishBusiness.ViewModels
{
    public class MerchantDetailsVm
    {
        public Merchant Merchant { get; set; }
        public List<MerchantReciept> MerchantReciepts { get; set; }
        public List<IMerchantReciept> IMerchantReciepts { get; set; }
        public List<ISellerReciept> ISellerReciepts { get; set; }
        public List<ISellerReciept> ISellerRecieptsMoneytized { get; set; }
        public List<PaidForMerchant> PaidForMerchantsFromUs { get; set; }
        public List<PaidForMerchant> PaidForUs { get; set; }
        public List<PaidForSeller> PaidForSeller { get; set; }

       
    }
}
