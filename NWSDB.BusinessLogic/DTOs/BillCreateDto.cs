namespace NWSDB.BusinessLogic.DTOs
{
    public class BillCreateDto
    {
        public int CustomerId { get; set; }
        public string BillingPeriod { get; set; } = string.Empty;
        public int PreviousReading { get; set; }
        public int CurrentReading { get; set; }
        public decimal UsageCharge { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal OtherCharges { get; set; }
    }
}
