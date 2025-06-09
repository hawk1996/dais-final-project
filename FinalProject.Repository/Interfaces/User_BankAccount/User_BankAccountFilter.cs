using System.Data.SqlTypes;

namespace FinalProject.Repository.Interfaces.User_BankAccount
{
    public class User_BankAccountFilter
    {
        public SqlInt32? UserId { get; set; }
        public SqlInt32? BankAccountId { get; set; }
    }
}
