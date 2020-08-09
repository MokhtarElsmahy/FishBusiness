using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Boat
    {
        [Key]
        public int BoatID { get; set; }

        [Required(ErrorMessage ="ادخل اسم المركب")]
        [Display(Name ="اسم المركب")]
        public string BoatName { get; set; }

        [Required(ErrorMessage = "ادخل اسم ريس المركب")]
        [Display(Name = "ريس المركب")]
        public string BoatLeader { get; set; }


       
        [Display(Name = "ديون الهالك")]
        public decimal DebtsOfHalek { get; set; }

        //[Display(Name = "ديون اعطال")]
        //public decimal DebtsOfMulfunction { get; set; }

        [Display(Name = "ايراد المركب الشريك")]
        public decimal IncomeOfSharedBoat { get; set; }

        [Display(Name = "اجمالى المصروفات")]
        public decimal TotalOfExpenses { get; set; }


        [Display(Name = "ديون الشغل")]
        public decimal DebtsOfStartingWork { get; set; }

        [Required(ErrorMessage = "ادخل رخصة المركب")]
        [Display(Name = "رخصة المركب")]
        public string BoatLicenseNumber { get; set; }

        [Required(ErrorMessage = "ادخل رقم المركب")]
        [Display(Name = " رقم المركب")]
        public string BoatNumber { get; set; }

        [Display(Name = " صورة المركب")]
        public string BoatImage { get; set; }

        [ForeignKey("BoatType")]
        public int TypeID { get; set; }
        public virtual BoatType BoatType { get; set; }


        public bool IsActive { get; set; }

        public virtual ICollection<BoatOwnerReciept> BoatOwnerReciepts { get; set; }
        public virtual ICollection<MerchantRecieptItem> MerchantRecieptItems { get; set; }
        public virtual ICollection<Sarha> Sarhas { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<SharedBoatsIncome> SharedBoatsIncomes { get; set; }
        public virtual ICollection<ExternalReceipt> ExternalReceipts { get; set; }
        public virtual ICollection<IncomesOfSharedBoat> IncomesOfSharedBoats { get; set; }




    }
}
