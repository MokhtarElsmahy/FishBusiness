using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class HalakaSellRecVm
    {
        public IEnumerable<Stock> Stocks { get; set; }
        public HalakSellReciept HalakSellReciept { get; set; }
    }
}
