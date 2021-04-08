using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class BoatRecBuyers
    {
        public string buyerName { get; set; }
        public string FishName { get; set; }
        public string ProductionType { get; set; }
        public string Qty { get; set; }

       
        public decimal unitPrice { get; set; }

      
    }
}
