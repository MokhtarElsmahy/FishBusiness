﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class ExternalReceipt
    {
        [Key]
        public int ExternalReceiptID { get; set; }

        [ForeignKey("Boat")]
        public int BoatID { get; set; }
        public virtual Boat Boat { get; set; }
        [ForeignKey("Sarha")]
        public int SarhaID { get; set; }
        [Required]
        public virtual Sarha Sarha { get; set; }
        [Display(Name = "انتاج السرحة")]
        [Required(ErrorMessage = "برجاء ادخال انتاج السرحة قبل العمولة")]
        public decimal TotalBeforePaying { get; set; }

        [Required(ErrorMessage = "برجاء ادخال تاريخ الفاتورة")]
        [Display(Name = "تاريخ الفاتوره")]
        public DateTime Date { get; set; }

        [Display(Name = "العمولة الخارجية")]
        [Required(ErrorMessage = "برجاء ادخال العمولة الخارجية")]

        public decimal Commission { get; set; }

        [Required(ErrorMessage = "برجاء ادخال المدفوع من الهالك")]
        [Display(Name = "المدفوع من الهالك")]
        public decimal PaidFromDebts { get; set; }

        [Display(Name = "صافى انتاج السرحة")]
        public decimal TotalAfterPaying { get; set; }
        [Display(Name = "ايراد السرحة")]
        public decimal FinalIncome { get; set; }

    }
}
