using System.Data.SqlTypes;

namespace FinalProject.Repository.Interfaces.BankAccount
{
    public class BankAccountFilter
    {
        public SqlString? AccountNumber { get; set; }
    }
}
