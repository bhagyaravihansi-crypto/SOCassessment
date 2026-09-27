using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class CustomersModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomersModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public string StatusFilter { get; set; } = "All";

    public int TotalCustomers { get; set; }
    public int ActiveCustomers { get; set; }
    public int InactiveCustomers { get; set; }
    public int NewCustomers { get; set; }

    public List<CustomerItem> Customers { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("NwsdbApi");

            var response = await client.GetAsync("api/Customers");

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage =
                    $"API returned status code: {(int)response.StatusCode} {response.StatusCode}";

                return;
            }

            var apiCustomers =
                await response.Content.ReadFromJsonAsync<List<ApiCustomer>>();

            if (apiCustomers == null)
            {
                ErrorMessage = "No customer data was returned from the API.";
                return;
            }

            Customers = apiCustomers.Select(c => new CustomerItem
            {
                Id = c.Id,
                CustomerId = c.AccountNumber,
                FullName = c.FullName,
                NIC = c.NIC,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                Status = c.Status,
                RegistrationDate = c.RegistrationDate
            }).ToList();

            // Summary
            TotalCustomers = Customers.Count;

            ActiveCustomers = Customers.Count(c =>
                string.Equals(
                    c.Status,
                    "Active",
                    StringComparison.OrdinalIgnoreCase));

            InactiveCustomers = Customers.Count(c =>
                string.Equals(
                    c.Status,
                    "Inactive",
                    StringComparison.OrdinalIgnoreCase));

            NewCustomers = Customers.Count(c =>
                c.RegistrationDate >= DateTime.Today.AddMonths(-1));

            // Search
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var search = SearchTerm.Trim();

                Customers = Customers
                    .Where(c =>
                        c.FullName.Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        c.CustomerId.Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        c.Email.Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Status filter
            if (!string.Equals(
                    StatusFilter,
                    "All",
                    StringComparison.OrdinalIgnoreCase))
            {
                Customers = Customers
                    .Where(c =>
                        string.Equals(
                            c.Status,
                            StatusFilter,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage =
                $"Unable to load customers: {ex.Message}";
        }
    }
}


// API response model
public class ApiCustomer
{
    public int Id { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string NIC { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; }
}


// UI model
public class CustomerItem
{
    public int Id { get; set; }

    public string CustomerId { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string NIC { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; }
}