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
       
    }
}
