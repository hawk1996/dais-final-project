namespace FinalProject.Web.Models.Payment
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}
