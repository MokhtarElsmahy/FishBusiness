using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class HalakaBuyRecDetailsVm
    {
        public HalakaBuyReciept halakaBuyReciept { get; set; }
        //public IEnumerable<IMerchantRecieptItem> ImerchantRecieptItems { get; set; }


        public List<HalakaBuyRecieptItem> NormalIMerchantItems { get; set; }
        public List<HalakaBuyRecieptItem> AmountIMerchantItems { get; set; }

        public IEnumerable<AmountVm> Amounts { get; set; }
    }
}
