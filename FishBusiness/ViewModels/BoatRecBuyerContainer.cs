using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class BoatRecBuyerContainer
    {
        public List<BoatRecBuyers> BoatRecBuyers { get; set; }
        public string BoatName { get; set; }
        public string date { get; set; }

        public List<MerchantRecieptItem> NormalMerchantItems { get; set; }
        public List<MerchantRecieptItem> AmountMerchantItems { get; set; }

        public IEnumerable<AmountVm> Amounts { get; set; }
    }
}
