using FinalProject.Model;
using FinalProject.Model.Enums;
using FinalProject.Repository.Base;
using FinalProject.Repository.Interfaces.Payment;
using Microsoft.Data.SqlClient;

namespace FinalProject.Repository.Implementations
{
    public class PaymentRepository : BaseRepository<Payment, PaymentFilter, PaymentUpdate>, IPaymentRepository
    {
        protected override string[] GetColumns() => new[]
{
            "UserId",
            "FromBankAccountId",
            "ToBankAccountId",
            "Amount",
            "Timestamp",
            "Reason",
            "Status"
        };

        protected override string GetIdColumnName() => "PaymentId";

        protected override string GetTableName() => "Payments";

        protected override Payment MapEntity(SqlDataReader reader)
        {
            return new Payment
            {
                PaymentId = Convert.ToInt32(reader["PaymentId"]),
                UserId = Convert.ToInt32(reader["UserId"]),
                FromBankAccountId = Convert.ToInt32(reader["FromBankAccountId"]),
                ToBankAccountId = Convert.ToInt32(reader["ToBankAccountId"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                Timestamp = Convert.ToDateTime(reader["Timestamp"]),
                Reason = Convert.ToString(reader["Reason"]),
                Status = (PaymentStatus)Convert.ToInt32(reader["Status"])
            };
        }
    }
}
