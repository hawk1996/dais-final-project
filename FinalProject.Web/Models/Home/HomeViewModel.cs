namespace FinalProject.Web.Models.Home
{
    public class HomeViewModel
    {
        public List<BankAccountViewModel> BankAccounts { get; set; } = new();
    }

    public class BankAccountViewModel
    {
        public int BankAccountId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
    }

}
