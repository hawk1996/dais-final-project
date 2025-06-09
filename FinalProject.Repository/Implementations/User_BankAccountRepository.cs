using FinalProject.Model;
using FinalProject.Repository.Base;
using FinalProject.Repository.Interfaces.User_BankAccount;
using Microsoft.Data.SqlClient;

namespace FinalProject.Repository.Implementations
{
    public class User_BankAccountRepository : BaseRepository<User_BankAccount, User_BankAccountFilter, User_BankAccountUpdate>, IUser_BankAccountRepository
    {

        protected override string[] GetColumns() => new[]
        {
            "UserId",
            "BankAccountId"
        };

        protected override string GetIdColumnName() => "Id";

        protected override string GetTableName() => "Users_BankAccounts";

        protected override User_BankAccount MapEntity(SqlDataReader reader)
        {
            return new User_BankAccount
            {
                Id = Convert.ToInt32(reader["Id"]),
                UserId = Convert.ToInt32(reader["UserId"]),
                BankAccountId = Convert.ToInt32(reader["BankAccountId"])
            };
        }
    }
}
