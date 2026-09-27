namespace NWSDB.BusinessLogic.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public string PaymentReference { get; set; }
            = string.Empty;

        public int CustomerId { get; set; }

        public int? BillId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }
            = string.Empty;

        public string Status { get; set; }
            = string.Empty;
    }
}