using FishBusiness.Models;
using System;
using System.Linq;

namespace FishBusiness.Controllers
{
    public class AmountViewModel
    {
        public Guid? AmountId { get; set; }
        public IGrouping<Guid?, int> items { get; set; }
    }
}