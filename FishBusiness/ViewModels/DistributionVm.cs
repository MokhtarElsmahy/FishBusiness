using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Controllers;
using FishBusiness.Models;

namespace FishBusiness.ViewModels
{
    public class DistributionVm
    {
        public BoatOwnerReciept BoatOwnerReciept { get; set; }
        public List<BoatOwnerItem>  NormalboatOwnerItems { get; set; }
        public List<BoatOwnerItem>  AmountboatOwnerItems { get; set; }
        public IEnumerable<AmountViewModel> Amounts { get; set; }
    }
}
