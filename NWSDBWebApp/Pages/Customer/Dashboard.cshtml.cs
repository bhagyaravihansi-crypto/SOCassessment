using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NWSDB.DataAccess;

public class DashboardModel : PageModel
{
    private readonly NwsdbContext _context;

    public DashboardModel(NwsdbContext context)
    {
        _context = context;
    }

    // Customer Details
    public string CustomerName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public DateTime MemberSince { get; set; }

    // Bill Details
    public decimal CurrentBill { get; set; }
    public DateTime DueDate { get; set; }
    public string BillingPeriod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;

    // Usage Details
    public int CurrentUsage { get; set; }
    public int PreviousUsage { get; set; }

    // Payment Details
    public decimal LastPaymentAmount { get; set; }
    public DateTime LastPaymentDate { get; set; }

    // Recent Payments
    public List<PaymentRecord> RecentPayments { get; set; }
        = new List<PaymentRecord>();

    public async Task<IActionResult> OnGetAsync()
    {
        // Get logged-in User ID from Session
        int? userId = HttpContext.Session.GetInt32("UserId");

        // User is not logged in
        if (userId == null)
        {
            return RedirectToPage("/Login");
        }

        // Get logged-in User
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId.Value);

        if (user == null)
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }

        // Get Customer using Account Number
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.AccountNumber == user.AccountNumber);

        if (customer == null)
        {
            return Page();
        }

        // CUSTOMER DETAILS

        CustomerName = customer.FullName;
        AccountNumber = customer.AccountNumber;
        MemberSince = customer.RegistrationDate;

        // CURRENT BILL

        var latestBill = await _context.Bills
            .Where(b => b.CustomerId == customer.Id)
            .OrderByDescending(b => b.IssueDate)
            .FirstOrDefaultAsync();

        if (latestBill != null)
        {
            CurrentBill = latestBill.TotalAmount;
            DueDate = latestBill.DueDate;
            BillingPeriod = latestBill.BillingPeriod;
            PaymentStatus = latestBill.Status;
        }

        // RECENT PAYMENTS

        var payments = await _context.Payments
            .Where(p => p.CustomerId == customer.Id)
            .OrderByDescending(p => p.PaymentDate)
            .Take(3)
            .ToListAsync();

        RecentPayments = payments.Select(p => new PaymentRecord
        {
            Date = p.PaymentDate,
            Reference = p.PaymentReference,
            Amount = p.Amount,
            Method = p.PaymentMethod,
            Status = p.Status
        }).ToList();

        // Latest payment
        var latestPayment = payments.FirstOrDefault();

        if (latestPayment != null)
        {
            LastPaymentAmount = latestPayment.Amount;
            LastPaymentDate = latestPayment.PaymentDate;
        }

        // WATER USAGE

        var latestUsage = await _context.UsageRecords
            .Where(u => u.CustomerId == customer.Id)
            .OrderByDescending(u => u.ReadingDate)
            .FirstOrDefaultAsync();

        if (latestUsage != null)
        {
            CurrentUsage = latestUsage.UnitsConsumed;
            PreviousUsage = latestUsage.PreviousReading;

            // Use latest usage billing period
            if (string.IsNullOrEmpty(BillingPeriod))
            {
                BillingPeriod = latestUsage.BillingMonth;
            }
        }

        return Page();
    }
}


// PAYMENT RECORD MODEL

public class PaymentRecord
{
    public DateTime Date { get; set; }

    public string Reference { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Method { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}