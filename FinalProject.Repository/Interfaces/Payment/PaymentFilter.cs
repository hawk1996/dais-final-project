using System.Data.SqlTypes;
using FinalProject.Model.Enums;

namespace FinalProject.Repository.Interfaces.Payment
{
    public class PaymentFilter
    {
        public SqlInt32? UserId { get; set; }
        public SqlInt32? FromBankAccountId { get; set; }
        public SqlInt32? ToBankAccountId { get; set; }
        public SqlString? Reason { get; set; }
        public PaymentStatus? Status { get; set; }
    }
}
