using FinalProject.Model;
using FinalProject.Model.Enums;
using FinalProject.Repository.Implementations;
using FinalProject.Repository;
using FinalProject.Repository.Interfaces.BankAccount;
using FinalProject.Repository.Interfaces.Payment;
using FinalProject.Services.DTOs.Payment;
using FinalProject.Services.Interfaces;
using FinalProject.Services.Helpers;

namespace FinalProject.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserService _userService;
        private readonly IBankAccountRepository _bankAccountRepository;
        public PaymentService (
            IPaymentRepository paymentRepository,
            IUserService userService,
            IBankAccountRepository bankAccountRepository)
        {
            _paymentRepository = paymentRepository;
            _userService = userService;
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<CreatePaymentResponse> CreateAsync(CreatePaymentRequest request)
        {
            var response = new CreatePaymentResponse();

            if (request.FromBankAccountId == request.ToBankAccountId)
            {
                response.ErrorMessage = "The transfer must be between different accounts";
                return response;
            }

            if (request.Amount <= 0 )
            {
                response.ErrorMessage = "The amount transferred must be positive";
                return response;
            }

            var fromBankAccount = await _bankAccountRepository.RetrieveByIdAsync(request.FromBankAccountId);
            if (fromBankAccount == null )
            {
                response.ErrorMessage = "The bank account to transfer from doesn't exist";
                return response;
            }

            var toBankAccount = await _bankAccountRepository.RetrieveByIdAsync(request.ToBankAccountId);
            if (toBankAccount == null)
            {
                response.ErrorMessage = "The bank account to transfer to doesn't exist";
                return response;
            }

            var userBankAccounts = await _userService.GetUserBankAccountsAsync(request.UserId);
            if (!userBankAccounts.BankAccounts.Any(e => e.BankAccountId == request.FromBankAccountId))
            {
                response.ErrorMessage = "You can only pay using your own bank accounts";
                return response;
            }

            response.PaymentId = await _paymentRepository.CreateAsync(new Payment
            {
                UserId = request.UserId,
                FromBankAccountId = request.FromBankAccountId,
                ToBankAccountId = request.ToBankAccountId,
                Amount = request.Amount,
                Reason = request.Reason,
                Timestamp = DateTime.Now,
                Status = PaymentStatus.Pending
            });

            response.Success = true;
            return response;
        }

        public async Task<GetPaymentsByUserResponse> GetPaymentsByUserAsync(int userId)
        {
            var payments = new List<PaymentDto>();
            var filter = new PaymentFilter { UserId = userId };
            await foreach (var payment in _paymentRepository.RetrieveCollectionAsync(filter))
            {
                if (payment != null)
                    payments.Add(MappingHelper.MapToDto(payment));
            }

            return new GetPaymentsByUserResponse { Payments = payments };
        }

        public async Task<ConfirmPaymentResponse> ConfirmAsync(ConfirmPaymentRequest request)
        {
            var response = new ConfirmPaymentResponse();
            var payment = await _paymentRepository.RetrieveByIdAsync(request.PaymentId);
            if (payment == null)
            {
                response.ErrorMessage = "Payment not found";
                return response;
            }

            if (payment.UserId != request.UserId)
            {
                response.ErrorMessage = "Can only confirm your own payments";
                return response;
            }

            if (payment.Status != PaymentStatus.Pending)
            {
                response.ErrorMessage = "The payment must be in status Pending";
                return response;
            }

            var fromBankAccount = await _bankAccountRepository.RetrieveByIdAsync(payment.FromBankAccountId);
            if (fromBankAccount == null)
            {
                response.ErrorMessage = "The bank account to transfer from doesn't exist";
                return response;
            }

            if (payment.Amount > fromBankAccount.Balance)
            {
                response.ErrorMessage = "Insufficient funds: the payment amount exceeds the balance of the selected bank account";
                return response;
            }

            var toBankAccount = await _bankAccountRepository.RetrieveByIdAsync(payment.ToBankAccountId);
            if (toBankAccount == null)
            {
                response.ErrorMessage = "The bank account to transfer to doesn't exist";
                return response;
            }

            var fromBankAccountUpdate = new BankAccountUpdate { Balance = fromBankAccount.Balance - payment.Amount };
            var toBankAccountUpdate = new BankAccountUpdate { Balance = toBankAccount.Balance + payment.Amount };
            var paymentUpdate = new PaymentUpdate { Status = PaymentStatus.Processed };
            await using var unitOfWork = new UnitOfWork();
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var success1 = await _bankAccountRepository.UpdateAsync(
                    fromBankAccount.BankAccountId,
                    fromBankAccountUpdate,
                    unitOfWork.Connection,
                    unitOfWork.Transaction);

                var success2 = await _bankAccountRepository.UpdateAsync(
                    toBankAccount.BankAccountId,
                    toBankAccountUpdate,
                    unitOfWork.Connection,
                    unitOfWork.Transaction);

                var success3 = await _paymentRepository.UpdateAsync(request.PaymentId, paymentUpdate, unitOfWork.Connection, unitOfWork.Transaction);

                if (!success1 || !success2 || !success3)
                    throw new Exception("One or more updates failed.");

                await unitOfWork.CommitAsync();
                response.Success = true;
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                response.ErrorMessage = "Failed to complete payment transaction. No changes were saved.";
            }

            return response;
        }

        public async Task<CancelPaymentResponse> CancelAsync(CancelPaymentRequest request)
        {
            var response = new CancelPaymentResponse();
            var payment = await _paymentRepository.RetrieveByIdAsync(request.PaymentId);
            if (payment == null)
            {
                response.ErrorMessage = "Payment not found";
                return response;
            }

            if (payment.UserId != request.UserId)
            {
                response.ErrorMessage = "Can only cancel your own payments";
                return response;
            }

            if (payment.Status != PaymentStatus.Pending)
            {
                response.ErrorMessage = "The payment must be in status Pending";
                return response;
            }

            var update = new PaymentUpdate { Status = PaymentStatus.Cancelled };
            response.Success = await _paymentRepository.UpdateAsync(request.PaymentId, update);
            if (!response.Success)
                response.ErrorMessage = "Failed to cancel payment";
            return response;
        }
    }
}
