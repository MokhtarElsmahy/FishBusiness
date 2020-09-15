using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseID { get; set; }

        [ForeignKey("Boat")]
        public int BoatID { get; set; }
        public virtual Boat Boat { get; set; }

        [Required(ErrorMessage ="برجاء ادخال التكلفة")]
        [Display(Name ="تكلفة")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "برجاء ادخال السبب")]
       
        [Display(Name = "السبب")]
        public string Cause { get; set; }

        [Required(ErrorMessage = "برجاء ادخال التاريخ")]
        [Display(Name = "التاريخ")]
        public DateTime Date { get; set; }

        public bool IsCheckedOut { get; set; }
    }
}
