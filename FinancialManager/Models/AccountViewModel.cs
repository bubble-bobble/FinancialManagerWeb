using System.ComponentModel.DataAnnotations;

namespace FinancialManager.Models
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public decimal Balance { get; set; }

        [Display(Name = "Account Type")]
        public int AccountTypeId { get; set; }
    }
}
