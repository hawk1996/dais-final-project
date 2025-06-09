using FinalProject.Model;
using FinalProject.Services.DTOs.BankAccount;
using FinalProject.Services.DTOs.Payment;

namespace FinalProject.Services.Helpers
{
    public static class MappingHelper
    {
        public static BankAccountDto? MapToDto(BankAccount? bankAccount) => bankAccount == null ? null : new BankAccountDto
        {
            BankAccountId = bankAccount.BankAccountId,
            AccountNumber = bankAccount.AccountNumber,
            Balance = bankAccount.Balance
        };

        public static PaymentDto MapToDto(Payment payment) => new PaymentDto
        {
            PaymentId = payment.PaymentId,
            FromBankAccountId = payment.FromBankAccountId,
            ToBankAccountId = payment.ToBankAccountId,
            Amount = payment.Amount,
            Reason = payment.Reason,
            Timestamp = payment.Timestamp,
            Status = payment.Status
        };
    }
}
