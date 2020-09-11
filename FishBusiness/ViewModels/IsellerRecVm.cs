using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class IsellerRecVm
    {
        public IEnumerable<Stock> Stocks { get; set; }
        public ISellerReciept ISellerReciept { get; set; }
    }
}
