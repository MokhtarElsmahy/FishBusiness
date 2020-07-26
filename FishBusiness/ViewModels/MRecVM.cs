using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class MRecVM
    {
        public MerchantReciept merchantReciept { get; set; }
        public Boat Boat { get; set; }
        public MerchantRecieptItem MerRecItem { get; set; }
       
    }
}
