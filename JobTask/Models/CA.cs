using System.ComponentModel.DataAnnotations;

namespace JobTask.Models
{
    public class CA
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        public string Number { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }
    }
}
