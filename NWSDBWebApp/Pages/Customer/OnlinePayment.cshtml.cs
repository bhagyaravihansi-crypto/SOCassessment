using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NWSDB.DataAccess;
using NWSDB.DataAccess.Entities;

namespace NWSDBWebApp.Pages.Customer
{
    public class OnlinePaymentModel : PageModel
    {
        private readonly NwsdbContext _context;

        public OnlinePaymentModel(NwsdbContext context)
        {
            _context = context;
        }

        // BILL / CUSTOMER INFORMATION

        public int BillId { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string AccountNumber { get; set; } = string.Empty;

        public string BillingPeriod { get; set; } = string.Empty;

        public decimal BillAmount { get; set; }

        public decimal ProcessingFee { get; set; } = 0m;

        public bool HasBill { get; set; }

        // PAYMENT FORM

        [BindProperty]
        [Required(ErrorMessage = "Enter payment amount")]
        [Range(
            0.01,
            1000000,
            ErrorMessage = "Enter a valid payment amount"
        )]
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

        [BindProperty]
        public bool SaveCard { get; set; }

        // MESSAGE

        public string? Message { get; set; }

        // TOTAL PAYABLE

        public decimal TotalPayable
        {
            get
            {
                return Amount + ProcessingFee;
            }
        }

        // GET

        public async Task<IActionResult> OnGetAsync(
            decimal? amount,
            int? billId)
        {
            return await LoadBillAsync(amount, billId);
        }

        
        // POST - PAY NOW

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Get logged-in user

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            // 2. Find logged-in user
            

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.Id == userId.Value);

            if (user == null)
            {
                HttpContext.Session.Clear();

                return RedirectToPage("/Account/Login");
            }

            // 3. Customer only

            if (!string.Equals(
                    user.Role,
                    "Customer",
                    StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/Admin/Dashboard");
            }

            // 4. Check account number

            if (string.IsNullOrWhiteSpace(user.AccountNumber))
            {
                Message = "No account number is associated with this user.";

                return Page();
            }

            // 5. Find customer

            var customer = await _context.Customers
                .FirstOrDefaultAsync(
                    c => c.AccountNumber == user.AccountNumber);

            if (customer == null)
            {
                Message = "Customer account was not found.";

                return Page();
            }

            // Set customer information
            CustomerId = customer.Id;
            CustomerName = customer.FullName;
            AccountNumber = customer.AccountNumber;

            // 6. Find bill

            Bill? bill;

            if (BillId > 0)
            {
                // Use selected bill
                bill = await _context.Bills
                    .FirstOrDefaultAsync(
                        b =>
                            b.Id == BillId &&
                            b.CustomerId == customer.Id);
            }
            else
            {
                // Otherwise use latest bill
                bill = await _context.Bills
                    .Where(
                        b => b.CustomerId == customer.Id)
                    .OrderByDescending(
                        b => b.IssueDate)
                    .ThenByDescending(
                        b => b.Id)
                    .FirstOrDefaultAsync();
            }

            // 7. Bill not found

            if (bill == null)
            {
                Message = "No bill was found for this customer.";

                return Page();
            }

            // Set bill information
            BillId = bill.Id;
            BillingPeriod = bill.BillingPeriod;
            BillAmount = bill.TotalAmount;
            HasBill = true;

            // 8. Validate payment form
            

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // 9. Create payment

            var payment = new Payment
            {
                PaymentReference =
                    $"PAY-{DateTime.Now:yyyyMMddHHmmssfff}",

                CustomerId = customer.Id,

                BillId = bill.Id,

                PaymentDate = DateTime.Now,

                Amount = Amount,

                PaymentMethod = PaymentMethod,

                Status = "Successful"
            };

            // 10. Save payment

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            // 11. Calculate total successful payments

            var totalPaid = await _context.Payments
                .Where(
                    p =>
                        p.BillId == bill.Id &&
                        p.Status == "Successful")
                .SumAsync(p => p.Amount);

            
            // 12. Update bill status

            if (totalPaid >= bill.TotalAmount)
            {
                bill.Status = "Paid";
            }
            else
            {
                bill.Status = "Pending";
            }

            // 13. Save bill status

            await _context.SaveChangesAsync();

            // 14. Success message

            Message =
                $"Payment of LKR {Amount:N2} was successful. " +
                $"Payment Reference: {payment.PaymentReference}";

            // 15. Clear sensitive card information

            CardNumber = null;
            CVV = null;
            Expiry = null;

            // 16. Return page

            return Page();
        }

        // LOAD BILL

        private async Task<IActionResult> LoadBillAsync(
            decimal? amount,
            int? billId)
        {
            // 1. Get logged-in user

            int? userId =
                HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            // 2. Find user

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.Id == userId.Value);

            if (user == null)
            {
                HttpContext.Session.Clear();

                return RedirectToPage("/Account/Login");
            }

           
            // 3. Customer only
            

            if (!string.Equals(
                    user.Role,
                    "Customer",
                    StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/Admin/Dashboard");
            }

            
            // 4. Account number check
            

            if (string.IsNullOrWhiteSpace(
                    user.AccountNumber))
            {
                return Page();
            }

            
            // 5. Find customer
            

            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    c =>
                        c.AccountNumber ==
                        user.AccountNumber);

            if (customer == null)
            {
                return Page();
            }

            // Set customer information
            CustomerId = customer.Id;
            CustomerName = customer.FullName;
            AccountNumber = customer.AccountNumber;

            
            // 6. Find bill
            

            Bill? bill;

            if (billId.HasValue &&
                billId.Value > 0)
            {
                // Selected bill
                bill = await _context.Bills
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        b =>
                            b.Id == billId.Value &&
                            b.CustomerId == customer.Id);
            }
            else
            {
                // Latest bill
                bill = await _context.Bills
                    .AsNoTracking()
                    .Where(
                        b => b.CustomerId == customer.Id)
                    .OrderByDescending(
                        b => b.IssueDate)
                    .ThenByDescending(
                        b => b.Id)
                    .FirstOrDefaultAsync();
            }

            
            // 7. No bill
            

            if (bill == null)
            {
                return Page();
            }

            
            // 8. Set bill information
            

            BillId = bill.Id;

            BillingPeriod =
                bill.BillingPeriod;

            BillAmount =
                bill.TotalAmount;

            HasBill = true;

            
            // 9. Set payment amount
            

            Amount =
                amount ?? BillAmount;

            return Page();
        }
    }
}