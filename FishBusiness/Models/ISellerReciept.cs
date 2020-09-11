using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class ISellerReciept
    {
        [Key]
        public int ISellerRecieptID { get; set; }

        [Display(Name = "تاريخ الفاتورة")]
        [Required(ErrorMessage = "برجاء ادخال تاريخ الفاتورة")]
        public DateTime Date { get; set; }
        [ForeignKey("Merchant")]
        public int MerchantID { get; set; }
        public virtual Merchant Merchant { get; set; }

        [Display(Name = "اجمالى سعر الفاتوره")]
        public double TotalOfPrices { get; set; }

        [Display(Name = "العموله")]
        public double Commision { get; set; }

        [Display(Name = "المدفوع من الديون")]
        public double PaidFromDebt { get; set; }

        [Display(Name = "الاجرة")]
        public double CarPrice { get; set; }

        [Display(Name = "البلد")]
        public string CarDistination { get; set; }


        [Display(Name = "تاريخ التسعير")]
    
        public DateTime DateOfMoneytization { get; set; }

        public virtual ICollection<ISellerRecieptItem> ISellerRecieptItems { get; set; }

    }
}
