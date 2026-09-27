using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class UsageModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public string BillingMonthFilter { get; set; } = "All";

    [BindProperty(SupportsGet = true)]
    public string StatusFilter { get; set; } = "All";

    public int TotalCustomers { get; set; } = 1250;
    public int TotalUsage { get; set; } = 28450; // m3
    public int AverageMonthlyUsage { get; set; } = 2371; // m3
    public int HighUsageAccounts { get; set; } = 42;

    public List<UsageRecord> UsageRecords { get; set; } = new List<UsageRecord>();

    public List<MonthlyUsage> Months { get; set; } = new List<MonthlyUsage>();

    public void OnGet()
    {
        Months = new List<MonthlyUsage>
        {
            new MonthlyUsage { Label = "Apr 2026", Usage = 2200 },
            new MonthlyUsage { Label = "May 2026", Usage = 2400 },
            new MonthlyUsage { Label = "Jun 2026", Usage = 2100 },
            new MonthlyUsage { Label = "Jul 2026", Usage = 2600 },
            new MonthlyUsage { Label = "Aug 2026", Usage = 2845 },
            new MonthlyUsage { Label = "Sep 2026", Usage = 2550 }
        };

        UsageRecords = new List<UsageRecord>
        {
            new UsageRecord { CustomerId = "C-1001", CustomerName = "Alice Perera", AccountNumber = "AC-1001", BillingMonth = "Aug 2026", PreviousReading = 12300, CurrentReading = 12312, UnitsConsumed = 12, UsageStatus = "Normal", ReadingDate = DateTime.Today.AddDays(-3) },
            new UsageRecord { CustomerId = "C-1002", CustomerName = "Buwan Silva", AccountNumber = "AC-1002", BillingMonth = "Aug 2026", PreviousReading = 15000, CurrentReading = 15040, UnitsConsumed = 40, UsageStatus = "High", ReadingDate = DateTime.Today.AddDays(-2) },
            new UsageRecord { CustomerId = "C-1003", CustomerName = "Chamara Fernando", AccountNumber = "AC-1003", BillingMonth = "Aug 2026", PreviousReading = 9800, CurrentReading = 9812, UnitsConsumed = 12, UsageStatus = "Normal", ReadingDate = DateTime.Today.AddDays(-7) },
            new UsageRecord { CustomerId = "C-1004", CustomerName = "Dilan Kumar", AccountNumber = "AC-1004", BillingMonth = "Jul 2026", PreviousReading = 2000, CurrentReading = 2015, UnitsConsumed = 15, UsageStatus = "Moderate", ReadingDate = DateTime.Today.AddDays(-30) },
            new UsageRecord { CustomerId = "C-1005", CustomerName = "Esha Jayawardena", AccountNumber = "AC-1005", BillingMonth = "Jun 2026", PreviousReading = 4000, CurrentReading = 4012, UnitsConsumed = 12, UsageStatus = "Normal", ReadingDate = DateTime.Today.AddMonths(-2) },
            new UsageRecord { CustomerId = "C-1006", CustomerName = "Farah Khan", AccountNumber = "AC-1006", BillingMonth = "Aug 2026", PreviousReading = 500, CurrentReading = 520, UnitsConsumed = 20, UsageStatus = "Moderate", ReadingDate = DateTime.Today.AddDays(-5) },
            new UsageRecord { CustomerId = "C-1007", CustomerName = "Gihan Perera", AccountNumber = "AC-1007", BillingMonth = "Aug 2026", PreviousReading = 7000, CurrentReading = 7030, UnitsConsumed = 30, UsageStatus = "High", ReadingDate = DateTime.Today.AddDays(-1) },
            new UsageRecord { CustomerId = "C-1008", CustomerName = "Himali Silva", AccountNumber = "AC-1008", BillingMonth = "May 2026", PreviousReading = 3500, CurrentReading = 3512, UnitsConsumed = 12, UsageStatus = "Normal", ReadingDate = DateTime.Today.AddMonths(-3) }
        };

        // Simple UI-only filtering
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            var q = SearchTerm.Trim().ToLowerInvariant();
            UsageRecords = UsageRecords.FindAll(u => u.CustomerName.ToLowerInvariant().Contains(q) || u.AccountNumber.ToLowerInvariant().Contains(q) || u.CustomerId.ToLowerInvariant().Contains(q));
        }

        if (!string.Equals(BillingMonthFilter, "All", StringComparison.OrdinalIgnoreCase))
        {
            UsageRecords = UsageRecords.FindAll(u => string.Equals(u.BillingMonth, BillingMonthFilter, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.Equals(StatusFilter, "All", StringComparison.OrdinalIgnoreCase))
        {
            UsageRecords = UsageRecords.FindAll(u => string.Equals(u.UsageStatus, StatusFilter, StringComparison.OrdinalIgnoreCase));
        }
    }
}

public class UsageRecord
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string BillingMonth { get; set; } = string.Empty;
    public int PreviousReading { get; set; }
    public int CurrentReading { get; set; }
    public int UnitsConsumed { get; set; }
    public string UsageStatus { get; set; } = string.Empty;
    public DateTime ReadingDate { get; set; }
}

public class MonthlyUsage
{
    public string Label { get; set; } = string.Empty;
    public int Usage { get; set; }
}
