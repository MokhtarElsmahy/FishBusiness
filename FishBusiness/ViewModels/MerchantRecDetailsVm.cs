using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;


namespace FishBusiness.ViewModels
{
    public class MerchantRecDetailsVm
    {
        public MerchantReciept MerchantReciept { get; set; }
        public List<MerchantRecieptItem> NormalMerchantItems { get; set; }
        public List<MerchantRecieptItem> AmountMerchantItems { get; set; }

        public string FromMerchnat { get; set; }

        public IEnumerable<AmountVm> Amounts { get; set; }

    }
}
