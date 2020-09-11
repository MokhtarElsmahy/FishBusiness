using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;

namespace FishBusiness.ViewModels
{
    public class ImerchRecDetailsVm
    {
        public IMerchantReciept ImerchantReciept { get; set; }
        public IEnumerable<IMerchantRecieptItem> ImerchantRecieptItems { get; set; }
    }
}
