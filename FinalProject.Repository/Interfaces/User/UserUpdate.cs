using System.Data.SqlTypes;

namespace FinalProject.Repository.Interfaces.User
{
    public class UserUpdate
    {
        public SqlString? Name { get; set; }
        public SqlString? Username { get; set; }
        public SqlString? Password { get; set; }
    }
}
