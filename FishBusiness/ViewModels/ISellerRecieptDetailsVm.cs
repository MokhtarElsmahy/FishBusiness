using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;

namespace FishBusiness.ViewModels
{
    public class ISellerRecieptDetailsVm
    {
        public ISellerReciept ISellerReciept { get; set; }
        public IEnumerable<ISellerRecieptItem> ISellerRecieptItems { get; set; }
    }
}
