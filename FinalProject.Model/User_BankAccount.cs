using System.ComponentModel.DataAnnotations;

namespace FinalProject.Model
{
    public class User_BankAccount
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Bank account is required")]
        public int BankAccountId { get; set; }
    }
}
