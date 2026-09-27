using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NWSDB.DataAccess;

namespace NWSDBWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NwsdbContext _context;

        public int CustomerCount { get; set; }

        public int PendingBillCount { get; set; }

        public int PaymentCount { get; set; }

        public IndexModel(NwsdbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            // Total customers
            CustomerCount = await _context.Customers.CountAsync();

            // Pending / Unpaid bills
            PendingBillCount = await _context.Bills
                .CountAsync(b =>
                    b.Status == "Pending" ||
                    b.Status == "Unpaid");

            // Total payments
            PaymentCount = await _context.Payments.CountAsync();
        }
    }
}