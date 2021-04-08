using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class HalakaBuyRecVm
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string SellerName { get; set; }
        public decimal TotalOfReciept { get; set; }
        public string FishNames { get; set; }
        public string ProductionTypes { get; set; }
        public string qtys { get; set; }
        public string unitprices { get; set; }
    }
}
