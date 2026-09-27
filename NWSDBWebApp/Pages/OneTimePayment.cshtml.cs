using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NWSDB.DataAccess;
using NWSDB.DataAccess.Entities;

namespace NWSDBWebApp.Pages
{
    public class OneTimePaymentModel : PageModel
    {
        private readonly NwsdbContext _context;

        public OneTimePaymentModel(NwsdbContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = "Enter NWSDB account number")]
        public string AccountNumber { get; set; } = string.Empty;

        [BindProperty]
        public string CustomerName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Enter payment amount")]
        [Range(0.01, 1000000,
            ErrorMessage = "Enter a valid payment amount")]
        public decimal Amount { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Select a payment method")]
        public string PaymentMethod { get; set; } = "Card";

        [BindProperty]
        public string? CardholderName { get; set; }

        [BindProperty]
        public string? CardNumber { get; set; }

        [BindProperty]
        public string? Expiry { get; set; }

        [BindProperty]
        public string? CVV { get; set; }

        public string? Message { get; set; }

        public bool PaymentSuccessful { get; set; }

        public void OnGet()
        {
            // Test customer information
            AccountNumber = "NWS10001";
            CustomerName = "Kasun Perera";
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Find customer from database
            var customer = _context.Customers
                .FirstOrDefault(c =>
                    c.AccountNumber == AccountNumber);

            if (customer == null)
            {
                Message = "NWSDB account number was not found.";
                PaymentSuccessful = false;

                return Page();
            }

            // Create payment
            var payment = new Payment
            {
                PaymentReference =
                    $"OTP-{DateTime.Now:yyyyMMddHHmmssfff}",

                CustomerId = customer.Id,

                BillId = null,

                PaymentDate = DateTime.Now,

                Amount = Amount,

                PaymentMethod = PaymentMethod,

                Status = "Successful"
            };

            _context.Payments.Add(payment);

            _context.SaveChanges();

            // Success message
            Message =
                $"Payment of LKR {Amount:N2} was successful. " +
                $"Payment Reference: {payment.PaymentReference}";

            PaymentSuccessful = true;

            // Clear sensitive card fields
            CardNumber = null;
            CVV = null;
            Expiry = null;

            return Page();
        }
    }
}