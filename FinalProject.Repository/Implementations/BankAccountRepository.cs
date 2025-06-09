using FinalProject.Model;
using FinalProject.Repository.Base;
using FinalProject.Repository.Interfaces.BankAccount;
using Microsoft.Data.SqlClient;

namespace FinalProject.Repository.Implementations
{
    public class BankAccountRepository : BaseRepository<BankAccount, BankAccountFilter, BankAccountUpdate>, IBankAccountRepository
    {
        protected override string[] GetColumns() => new[]
        {
            "AccountNumber",
            "Balance"
        };

        protected override string GetIdColumnName() => "BankAccountId";

        protected override string GetTableName() => "BankAccounts";

        protected override BankAccount MapEntity(SqlDataReader reader)
        {
            return new BankAccount
            {
                BankAccountId = Convert.ToInt32(reader["BankAccountId"]),
                AccountNumber = Convert.ToString(reader["AccountNumber"]),
                Balance = Convert.ToDecimal(reader["Balance"])
            };
        }
    }
}
}
