using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class FathAllahMainProfileVM
    {
        public ICollection<LeaderPayback> LeaderPaybacks { get; set; }
        public ICollection<LeaderLoan> LeaderLoan { get; set; }
        public ICollection<FathAllahGift> FathAllahGifts { get; set; }
    }
}
