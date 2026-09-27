using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class PaymentsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public string MethodFilter { get; set; } = "All";

    [BindProperty(SupportsGet = true)]
    public string StatusFilter { get; set; } = "All";

    public int TotalPayments { get; set; } = 1164;
    public int SuccessfulPayments { get; set; } = 1080;
    public int PendingPayments { get; set; } = 60;
    public int FailedPayments { get; set; } = 24;

    public List<PaymentItem> Payments { get; set; } = new List<PaymentItem>();

    public List<PaymentActivityItem> RecentActivity { get; set; } = new List<PaymentActivityItem>();

    public void OnGet()
    {
        Payments = new List<PaymentItem>
        {
            new PaymentItem { PaymentId = "PAY-3001", CustomerId = "AC-1001", CustomerName = "Alice Perera", BillId = "BILL-2001", PaymentDate = DateTime.Today, PaymentMethod = "Card", Amount = 72.45m, Reference = "REF-9001", Status = "Successful" },
            new PaymentItem { PaymentId = "PAY-3002", CustomerId = "AC-1002", CustomerName = "Buwan Silva", BillId = "BILL-2002", PaymentDate = DateTime.Today.AddDays(-1), PaymentMethod = "Mobile", Amount = 120.00m, Reference = "REF-9002", Status = "Pending" },
            new PaymentItem { PaymentId = "PAY-3003", CustomerId = "AC-1003", CustomerName = "Chamara Fernando", BillId = "BILL-2003", PaymentDate = DateTime.Today.AddDays(-2), PaymentMethod = "Bank", Amount = 72.45m, Reference = "REF-9003", Status = "Failed" },
            new PaymentItem { PaymentId = "PAY-3004", CustomerId = "AC-1004", CustomerName = "Dilan Kumar", BillId = "BILL-2004", PaymentDate = DateTime.Today.AddDays(-3), PaymentMethod = "Card", Amount = 90.00m, Reference = "REF-9004", Status = "Successful" },
            new PaymentItem { PaymentId = "PAY-3005", CustomerId = "AC-1005", CustomerName = "Esha Jayawardena", BillId = "BILL-2005", PaymentDate = DateTime.Today.AddDays(-4), PaymentMethod = "Mobile", Amount = 72.45m, Reference = "REF-9005", Status = "Successful" },
            new PaymentItem { PaymentId = "PAY-3006", CustomerId = "AC-1006", CustomerName = "Farah Khan", BillId = "BILL-2006", PaymentDate = DateTime.Today.AddDays(-5), PaymentMethod = "Bank", Amount = 60.00m, Reference = "REF-9006", Status = "Pending" },
            new PaymentItem { PaymentId = "PAY-3007", CustomerId = "AC-1007", CustomerName = "Gihan Perera", BillId = "BILL-2007", PaymentDate = DateTime.Today.AddDays(-6), PaymentMethod = "Card", Amount = 45.00m, Reference = "REF-9007", Status = "Successful" },
            new PaymentItem { PaymentId = "PAY-3008", CustomerId = "AC-1008", CustomerName = "Himali Silva", BillId = "BILL-2008", PaymentDate = DateTime.Today.AddDays(-7), PaymentMethod = "Mobile", Amount = 30.00m, Reference = "REF-9008", Status = "Failed" }
        };

        RecentActivity = new List<PaymentActivityItem>
        {
            new PaymentActivityItem { Date = DateTime.Today, Description = "Payment received", Details = "PAY-3001 received for BILL-2001", Status = "Successful" },
            new PaymentActivityItem { Date = DateTime.Today.AddDays(-1), Description = "Payment verification pending", Details = "PAY-3002 awaiting confirmation", Status = "Pending" },
            new PaymentActivityItem { Date = DateTime.Today.AddDays(-2), Description = "Payment failed", Details = "PAY-3003 failed due to card decline", Status = "Failed" }
        };

        // Simple UI-only filtering
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            var q = SearchTerm.Trim().ToLowerInvariant();
            Payments = Payments.FindAll(p => p.CustomerName.ToLowerInvariant().Contains(q) || p.CustomerId.ToLowerInvariant().Contains(q) || p.Reference.ToLowerInvariant().Contains(q));
        }

        if (!string.Equals(MethodFilter, "All", StringComparison.OrdinalIgnoreCase))
        {
            Payments = Payments.FindAll(p => string.Equals(p.PaymentMethod, MethodFilter, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.Equals(StatusFilter, "All", StringComparison.OrdinalIgnoreCase))
        {
            Payments = Payments.FindAll(p => string.Equals(p.Status, StatusFilter, StringComparison.OrdinalIgnoreCase));
        }
    }
}

public class PaymentItem
{
    public string PaymentId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BillId { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class PaymentActivityItem
{
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
