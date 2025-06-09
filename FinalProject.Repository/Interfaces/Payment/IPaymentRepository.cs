using FinalProject.Repository.Base;

namespace FinalProject.Repository.Interfaces.Payment
{
    public interface IPaymentRepository : IBaseRepository<Model.Payment, PaymentFilter, PaymentUpdate>
    {
    }
}
