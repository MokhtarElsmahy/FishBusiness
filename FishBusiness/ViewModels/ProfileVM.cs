﻿using FishBusiness.Models;
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
        public IEnumerable<Expense> BoatExpenses { get; set; }

       


    }
}