using System.Data.SqlTypes;

namespace FinalProject.Repository.Interfaces.User
{
    public class UserFilter
    {
        public SqlString? Username { get; set; }
        public SqlString? Name { get; set; }
    }
}
