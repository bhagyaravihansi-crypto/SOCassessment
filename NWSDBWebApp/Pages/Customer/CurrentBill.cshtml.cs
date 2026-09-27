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
    public class CurrentBillModel : PageModel
    {
        private readonly NwsdbContext _db;

        public CurrentBillModel(NwsdbContext db)
        {
            _db = db;
        }

        public int BillId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string AccountNumber { get; set; } = string.Empty;

        public string BillingPeriod { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsOverdue => DueDate < DateTime.Today;

        public string PaymentStatus { get; set; } = "Pending";

        public decimal TotalDue { get; set; }

        public int PreviousReading { get; set; }

        public int CurrentReading { get; set; }

        public int UnitsConsumed { get; set; }

        public List<ChargeItem> Charges { get; set; } = new();

        public bool HasBill { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get logged-in UserId from Session
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            // Get logged-in user
            var user = await _db.Users
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

            // Account number is used to connect User -> Customer
            if (string.IsNullOrWhiteSpace(user.AccountNumber))
            {
                ErrorMessage = "No account number is associated with this user.";
                return Page();
            }

            // Find the customer using Account Number
            var customer = await _db.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.AccountNumber == user.AccountNumber);

            if (customer == null)
            {
                ErrorMessage = "Customer account could not be found.";
                return Page();
            }

            // Get the latest bill for this customer
            var bill = await _db.Bills
                .AsNoTracking()
                .Where(b => b.CustomerId == customer.Id)
                .OrderByDescending(b => b.IssueDate)
                .ThenByDescending(b => b.Id)
                .FirstOrDefaultAsync();

            if (bill == null)
            {
                CustomerName = customer.FullName;
                AccountNumber = customer.AccountNumber;
                HasBill = false;

                ErrorMessage = "There is no current bill available for this account.";

                return Page();
            }

            // Customer details
            CustomerName = customer.FullName;
            AccountNumber = customer.AccountNumber;

            // Bill details
            BillId = bill.Id;
            BillingPeriod = bill.BillingPeriod;
            IssueDate = bill.IssueDate;
            DueDate = bill.DueDate;

            PreviousReading = bill.PreviousReading;
            CurrentReading = bill.CurrentReading;
            UnitsConsumed = bill.UnitsConsumed;

            TotalDue = bill.TotalAmount;

            PaymentStatus = string.IsNullOrWhiteSpace(bill.Status)
                ? "Pending"
                : bill.Status;

            HasBill = true;

            // Charge breakdown
            Charges = new List<ChargeItem>
            {
                new ChargeItem
                {
                    Description = $"Water usage ({bill.UnitsConsumed} m³)",
                    Amount = bill.UsageCharge
                },

                new ChargeItem
                {
                    Description = "Service charge",
                    Amount = bill.ServiceCharge
                },

                new ChargeItem
                {
                    Description = "Other charges",
                    Amount = bill.OtherCharges
                }
            };

            return Page();
        }
    }

    public class ChargeItem
    {
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}

