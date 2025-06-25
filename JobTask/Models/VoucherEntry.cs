using System;
using System.ComponentModel.DataAnnotations;

namespace JobTask.Models
{
    public class VoucherEntries
    {
        [Required]
        public string VoucherType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime VoucherDate { get; set; }

        [Required]
        public string ReferenceNo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Account ID")]
        public int AccountID { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Debit amount must be non-negative number")]
        public decimal DebitAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Credit amount must be non-negative number")]
        public decimal CreditAmount { get; set; }
    }
}
