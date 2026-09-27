namespace NWSDB.BusinessLogic.DTOs
{
    public class PaymentCreateDto
    {
        public int? CustomerId { get; set; }

        public int? BillId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;
    }
}
