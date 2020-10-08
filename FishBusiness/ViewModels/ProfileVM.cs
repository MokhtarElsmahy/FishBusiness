using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class ProfileVM
    {
        public BoatInfoVM BoatInfo { get; set; }
        public IEnumerable<BoatOwnerReciept> BoatRecs { get; set; }
        public IEnumerable<BoatOwnerReciept> NotCalculatedRec { get; set; }
        public IEnumerable<ExternalReceipt> ExternalRecs { get; set; }
        public IEnumerable<Expense> BoatExpenses { get; set; }
        public IEnumerable<Sarha> Haleks { get; set; }

       


    }
}
