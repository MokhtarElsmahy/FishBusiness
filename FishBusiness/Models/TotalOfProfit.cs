using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class TotalOfProfit
    {
        [Key]
        public int ID { get; set; }
        public DateTime Date { get; set; }

        [Display(Name = "اجمالى مبيعات")]

        public double TotalOfSales  { get; set; }

        [Display(Name = "اجمالى مشتريات")]
        public double TotalOfPurchases  { get; set; }
        [Display(Name = "ارباح")]
        public double Profit  { get; set; }

        [Display(Name ="تلج")]
        public double Ice  { get; set; }
        [Display(Name = "عمال")]
        public double Labour  { get; set; }
    }
}
