using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;

namespace FishBusiness.ViewModels
{
    public class CheckoutVM
    {
        public IEnumerable<BoatOwnerReciept> BoatOwnerReciepts { get; set; }
        public IEnumerable<Expense> Expenses { get; set; }
    }
}
