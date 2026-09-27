using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NWSDB.DataAccess;
using NWSDB.DataAccess.Entities;

namespace NWSDBWebApp.Pages.Customer
{
    public class PaymentHistoryModel : PageModel
    {
        private readonly NwsdbContext _context;

        public PaymentHistoryModel(NwsdbContext context)
        {
            _context = context;
        }

        public string CustomerName { get; set; } = string.Empty;

        public string AccountNumber { get; set; } = string.Empty;

        public int TotalPayments { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal LastPaymentAmount { get; set; }

        public DateTime? LastPaymentDate { get; set; }

        public List<PaymentRecord> Payments { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get logged-in user ID from Session
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            // Find logged-in user
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            // Only customers can access this page
            if (!string.Equals(user.Role, "Customer", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/Admin/Dashboard");
            }

            if (string.IsNullOrWhiteSpace(user.AccountNumber))
            {
                return Page();
            }

            // Find customer using Account Number
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    c => c.AccountNumber == user.AccountNumber);

            if (customer == null)
            {
                return Page();
            }

            CustomerName = customer.FullName;
            AccountNumber = customer.AccountNumber;

            // Get all payments belonging to this customer
            var paymentList = await _context.Payments
                .AsNoTracking()
                .Where(p => p.CustomerId == customer.Id)
                .Include(p => p.Bill)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            // Convert database payments to page records
            Payments = paymentList
                .Select(p => new PaymentRecord
                {
                    Date = p.PaymentDate,

                    Reference = p.PaymentReference,

                    BillingPeriod = p.Bill != null
                        ? p.Bill.BillingPeriod
                        : "N/A",

                    Method = p.PaymentMethod,

                    Amount = p.Amount,

                    Status = p.Status
                })
                .ToList();

            // Summary values
            TotalPayments = Payments.Count;

            TotalPaid = Payments
                .Where(p =>
                    string.Equals(
                        p.Status,
                        "Successful",
                        StringComparison.OrdinalIgnoreCase))
                .Sum(p => p.Amount);

            if (Payments.Any())
            {
                var lastPayment = Payments.First();

                LastPaymentAmount = lastPayment.Amount;
                LastPaymentDate = lastPayment.Date;
            }

            return Page();
        }
    }

    public class PaymentRecord
    {
        public DateTime Date { get; set; }

        public string Reference { get; set; } = string.Empty;

        public string BillingPeriod { get; set; } = string.Empty;

        public string Method { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}

