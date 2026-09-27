using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.DataAccess;

public class BillsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly NwsdbContext _context;

    public BillsModel(
        IHttpClientFactory httpClientFactory,
        NwsdbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public string BillingPeriodFilter { get; set; } = "All";

    [BindProperty(SupportsGet = true)]
    public string StatusFilter { get; set; } = "All";

    // Create Bill Form

    [BindProperty]
    public int NewCustomerId { get; set; }

    [BindProperty]
    public string NewBillingPeriod { get; set; } = string.Empty;

    [BindProperty]
    public int NewPreviousReading { get; set; }

    [BindProperty]
    public int NewCurrentReading { get; set; }

    [BindProperty]
    public decimal NewUsageCharge { get; set; }

    [BindProperty]
    public decimal NewServiceCharge { get; set; }

    [BindProperty]
    public decimal NewOtherCharges { get; set; }

    public decimal NewTotalAmount =>
        NewUsageCharge +
        NewServiceCharge +
        NewOtherCharges;

    public List<CustomerOption> Customers { get; set; } = new();

    public string? SuccessMessage { get; set; }

    public string? ErrorMessage { get; set; }

   
    // Existing Bill Management
   

    public int TotalBills { get; set; }

    public int PendingBills { get; set; }

    public int PaidBills { get; set; }

    public int OverdueBills { get; set; }

    public List<BillItem> Bills { get; set; } = new();

    public List<string> BillingPeriods { get; set; } = new();

    // GET

    public async Task OnGetAsync()
    {
        await LoadCustomersAsync();
        await LoadBillsAsync();
    }

    // POST - Create Bill

    public async Task<IActionResult> OnPostCreateBillAsync()
    {
        // Load customer list again in case validation fails
        await LoadCustomersAsync();

        // Basic validation
        if (NewCustomerId <= 0)
        {
            ErrorMessage = "Please select a customer.";
            await LoadBillsAsync();
            return Page();
        }

        if (string.IsNullOrWhiteSpace(NewBillingPeriod))
        {
            ErrorMessage = "Please enter the billing period.";
            await LoadBillsAsync();
            return Page();
        }

        if (NewCurrentReading < NewPreviousReading)
        {
            ErrorMessage =
                "Current reading cannot be lower than previous reading.";

            await LoadBillsAsync();
            return Page();
        }

        if (NewUsageCharge < 0 ||
            NewServiceCharge < 0 ||
            NewOtherCharges < 0)
        {
            ErrorMessage = "Charges cannot be negative.";
            await LoadBillsAsync();
            return Page();
        }

        // Check customer exists
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == NewCustomerId);

        if (customer == null)
        {
            ErrorMessage = "Selected customer was not found.";
            await LoadBillsAsync();
            return Page();
        }

        // Calculate units
        int unitsConsumed =
            NewCurrentReading - NewPreviousReading;

        // Create bill
        var bill = new NWSDB.DataAccess.Entities.Bill
        {
            CustomerId = customer.Id,

            BillNumber =
                $"BILL-{DateTime.Now:yyyyMMddHHmmssfff}",

            BillingPeriod = NewBillingPeriod.Trim(),

            IssueDate = DateTime.Now,

            DueDate = DateTime.Now.AddDays(14),

            PreviousReading = NewPreviousReading,

            CurrentReading = NewCurrentReading,

            UnitsConsumed = unitsConsumed,

            UsageCharge = NewUsageCharge,

            ServiceCharge = NewServiceCharge,

            OtherCharges = NewOtherCharges,

            TotalAmount =
                NewUsageCharge +
                NewServiceCharge +
                NewOtherCharges,

            Status = "Unpaid"
        };

        // Save to database
        _context.Bills.Add(bill);

        await _context.SaveChangesAsync();

        SuccessMessage =
            $"Bill {bill.BillNumber} was successfully created for " +
            $"{customer.FullName}.";

        // Clear form
        NewCustomerId = 0;
        NewBillingPeriod = string.Empty;
        NewPreviousReading = 0;
        NewCurrentReading = 0;
        NewUsageCharge = 0;
        NewServiceCharge = 0;
        NewOtherCharges = 0;

        await LoadCustomersAsync();
        await LoadBillsAsync();

        return Page();
    }

    // Load Customers

    private async Task LoadCustomersAsync()
    {
        Customers = await _context.Customers
            .AsNoTracking()
            .Where(c => c.Status == "Active")
            .OrderBy(c => c.FullName)
            .Select(c => new CustomerOption
            {
                Id = c.Id,
                FullName = c.FullName,
                AccountNumber = c.AccountNumber
            })
            .ToListAsync();
    }

    // Load Bills
    

    private async Task LoadBillsAsync()
    {
        try
        {
            var client =
                _httpClientFactory.CreateClient("NwsdbApi");

            var apiBills =
                await client.GetFromJsonAsync<List<BillDto>>(
                    "api/Bills"
                ) ?? new List<BillDto>();

            Bills = apiBills
                .Select(b => new BillItem
                {
                    BillId = b.BillNumber,

                    CustomerId =
                        b.CustomerId.ToString(),

                    CustomerName =
                        b.CustomerName,

                    BillingPeriod =
                        b.BillingPeriod,

                    PreviousReading =
                        b.PreviousReading,

                    CurrentReading =
                        b.CurrentReading,

                    UnitsConsumed =
                        b.UnitsConsumed,

                    BillAmount =
                        b.TotalAmount,

                    DueDate =
                        b.DueDate,

                    Status =
                        NormalizeStatus(b.Status)
                })
                .ToList();

            // Billing periods
            BillingPeriods = Bills
                .Select(b => b.BillingPeriod)
                .Where(p =>
                    !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(p => p)
                .ToList();

            // Search
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var q = SearchTerm.Trim();

                Bills = Bills
                    .Where(b =>
                        b.CustomerName.Contains(
                            q,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        b.CustomerId.Contains(
                            q,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        b.BillId.Contains(
                            q,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Billing period filter
            if (!string.Equals(
                    BillingPeriodFilter,
                    "All",
                    StringComparison.OrdinalIgnoreCase))
            {
                Bills = Bills
                    .Where(b =>
                        string.Equals(
                            b.BillingPeriod,
                            BillingPeriodFilter,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Status filter
            if (!string.Equals(
                    StatusFilter,
                    "All",
                    StringComparison.OrdinalIgnoreCase))
            {
                Bills = Bills
                    .Where(b =>
                        string.Equals(
                            b.Status,
                            StatusFilter,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Counts
            TotalBills = Bills.Count;

            PaidBills = Bills.Count(b =>
                string.Equals(
                    b.Status,
                    "Paid",
                    StringComparison.OrdinalIgnoreCase));

            PendingBills = Bills.Count(b =>
                string.Equals(
                    b.Status,
                    "Pending",
                    StringComparison.OrdinalIgnoreCase));

            OverdueBills = Bills.Count(b =>
                string.Equals(
                    b.Status,
                    "Overdue",
                    StringComparison.OrdinalIgnoreCase));
        }
        catch
        {
            Bills = new List<BillItem>();

            BillingPeriods = new List<string>();

            TotalBills = 0;
            PendingBills = 0;
            PaidBills = 0;
            OverdueBills = 0;
        }
    }

    // Normalize Status

    private static string NormalizeStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return "Pending";

        if (status.Equals(
                "Paid",
                StringComparison.OrdinalIgnoreCase))
            return "Paid";

        if (status.Equals(
                "Overdue",
                StringComparison.OrdinalIgnoreCase))
            return "Overdue";

        if (status.Equals(
                "Unpaid",
                StringComparison.OrdinalIgnoreCase))
            return "Pending";

        if (status.Equals(
                "Pending",
                StringComparison.OrdinalIgnoreCase))
            return "Pending";

        return status;
    }
}

// Customer Dropdown Item

public class CustomerOption
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;
}

// Bill Display Item

public class BillItem
{
    public string BillId { get; set; } = string.Empty;

    public string CustomerId { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string BillingPeriod { get; set; } = string.Empty;

    public int PreviousReading { get; set; }

    public int CurrentReading { get; set; }

    public int UnitsConsumed { get; set; }

    public decimal BillAmount { get; set; }

    public DateTime DueDate { get; set; }

    public string Status { get; set; } = string.Empty;
}