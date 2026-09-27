using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NWSDB.DataAccess;

public class AdminDashboardModel : PageModel
{
    private readonly NwsdbContext _context;

    public AdminDashboardModel(NwsdbContext context)
    {
        _context = context;
    }

    public int TotalCustomers { get; set; }

    public int PendingBills { get; set; }

    public int TotalPayments { get; set; }

    public int TotalUsage { get; set; }

    public List<ActivityItem> RecentActivities { get; set; }
        = new List<ActivityItem>();

    public List<PendingItem> PendingItems { get; set; }
        = new List<PendingItem>();


    public void OnGet()
    {
        // Total Customers
        TotalCustomers = _context.Customers.Count();


        // Pending Bills
        PendingBills = _context.Bills
            .Count(b => b.Status == "Pending");


        // Total Payments
        TotalPayments = _context.Payments.Count();


        // Total Water Usage
        // Temporary calculation until the exact
        // UsageRecord property is confirmed.
        TotalUsage = _context.UsageRecords.Count();


        // Recent Payments
        var recentPayments = _context.Payments
            .OrderByDescending(p => p.PaymentDate)
            .Take(5)
            .Select(p => new ActivityItem
            {
                Date = p.PaymentDate,
                Activity = "Payment received",
                Customer = p.Customer != null
                    ? p.Customer.FullName
                    : "Guest Customer",
                Status = p.Status
            })
            .ToList();


        // Recent Customers
        var recentCustomers = _context.Customers
            .OrderByDescending(c => c.Id)
            .Take(5)
            .Select(c => new ActivityItem
            {
                Date = DateTime.Today,
                Activity = "Customer registered",
                Customer = c.FullName,
                Status = "Completed"
            })
            .ToList();


        // Combine recent activities
        RecentActivities = recentPayments
            .Concat(recentCustomers)
            .OrderByDescending(a => a.Date)
            .Take(6)
            .ToList();


        // Pending Actions
        PendingItems = new List<PendingItem>
        {
            new PendingItem
            {
                Description = "Bills awaiting payment",
                Count = PendingBills
            },

            new PendingItem
            {
                Description = "Customer registrations for review",
                Count = 0
            },

            new PendingItem
            {
                Description = "Payments requiring verification",
                Count = _context.Payments
                    .Count(p => p.Status == "Pending")
            }
        };
    }
}


public class ActivityItem
{
    public DateTime Date { get; set; }

    public string Activity { get; set; } = string.Empty;

    public string Customer { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}


public class PendingItem
{
    public string Description { get; set; } = string.Empty;

    public int Count { get; set; }
}