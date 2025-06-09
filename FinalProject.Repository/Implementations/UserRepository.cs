using FinalProject.Model;
using FinalProject.Repository.Base;
using FinalProject.Repository.Interfaces.User;
using Microsoft.Data.SqlClient;

namespace FinalProject.Repository.Implementations
{
    public class UserRepository : BaseRepository<User, UserFilter, UserUpdate>, IUserRepository
    {
        protected override string[] GetColumns() => new[]
        {
            "Name",
            "Username",
            "Password"
        };

        protected override string GetIdColumnName() => "UserId";

        protected override string GetTableName() => "Users";

        protected override User MapEntity(SqlDataReader reader)
        {
            return new User
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                Name = Convert.ToString(reader["Name"]),
                Username = Convert.ToString(reader["Username"]),
                Password = Convert.ToString(reader["Password"])
            };
        }
    }
}
