using System.Data.SqlTypes;

namespace FinalProject.Repository.Interfaces.BankAccount
{
    public class BankAccountUpdate
    {
        public SqlString? AccountNumber { get; set; }
        public SqlDecimal? Balance { get; set; }
    }
}
