using FinalProject.Repository.Base;

namespace FinalProject.Repository.Interfaces.User
{
    public interface IUserRepository : IBaseRepository<Model.User, UserFilter, UserUpdate>
    {
    }
}
