using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace NWSDBWebApp.Pages.Customer
{
    public class WaterUsageModel : PageModel
    {
        public string CustomerName { get; set; } = "Bhagya Ravihansi";

        public string AccountNumber { get; set; } = "NWSDB-100245";

        public string CurrentMonth { get; set; } = "September 2026";

        public double CurrentUsage { get; set; } = 24;

        public double PreviousUsage { get; set; } = 21;

        public string UsageStatus { get; set; } = "Moderate";

        public double PreviousMeterReading { get; set; } = 1263;

        public double CurrentMeterReading { get; set; } = 1287;

        public double MaximumUsage { get; set; } = 35;

        public List<MonthlyUsageData> MonthlyUsage { get; set; } = new();

        public void OnGet()
        {
            MonthlyUsage = new List<MonthlyUsageData>
            {
                new MonthlyUsageData
                {
                    Month = "April 2026",
                    PreviousReading = 1168,
                    CurrentReading = 1185,
                    Units = 17
                },
                new MonthlyUsageData
                {
                    Month = "May 2026",
                    PreviousReading = 1185,
                    CurrentReading = 1203,
                    Units = 18
                },
                new MonthlyUsageData
                {
                    Month = "June 2026",
                    PreviousReading = 1203,
                    CurrentReading = 1224,
                    Units = 21
                },
                new MonthlyUsageData
                {
                    Month = "July 2026",
                    PreviousReading = 1224,
                    CurrentReading = 1242,
                    Units = 18
                },
                new MonthlyUsageData
                {
                    Month = "August 2026",
                    PreviousReading = 1242,
                    CurrentReading = 1263,
                    Units = 21
                },
                new MonthlyUsageData
                {
                    Month = "September 2026",
                    PreviousReading = 1263,
                    CurrentReading = 1287,
                    Units = 24
                }
            };
        }
    }

    public class MonthlyUsageData
    {
        public string Month { get; set; } = string.Empty;

        public double PreviousReading { get; set; }

        public double CurrentReading { get; set; }

        public double Units { get; set; }
    }
}