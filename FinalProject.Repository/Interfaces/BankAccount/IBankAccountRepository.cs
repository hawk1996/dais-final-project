using FinalProject.Repository.Base;

namespace FinalProject.Repository.Interfaces.BankAccount
{
    public interface IBankAccountRepository : IBaseRepository<Model.BankAccount, BankAccountFilter, BankAccountUpdate>
    {
    }
}
