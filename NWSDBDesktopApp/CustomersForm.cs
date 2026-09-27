using System;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NWSDBDesktopApp
{
    public partial class CustomersForm : Form
    {
        private readonly HttpClient _httpClient;

        private DataGridView customersGrid = null!;
        private TextBox searchBox = null!;
        private Button refreshButton = null!;

        public CustomersForm()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7293/")
            };

            CreateCustomerUI();

            LoadCustomers();
        }

        private void CreateCustomerUI()
        {
            Text = "NWSDB - Customers";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1100, 650);
            BackColor = Color.White;

            
            // HEADER
            

            Panel header = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(ClientSize.Width, 70),
                BackColor = Color.FromArgb(0, 91, 170),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left |
                         AnchorStyles.Right
            };

            Controls.Add(header);

            Label title = new Label
            {
                Text = "Customer Management",
                Location = new Point(25, 18),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font(
                    "Segoe UI",
                    19,
                    FontStyle.Bold)
            };

            header.Controls.Add(title);

          
            // SEARCH
           

            Label searchLabel = new Label
            {
                Text = "Search Customer",
                Location = new Point(30, 100),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold)
            };

            Controls.Add(searchLabel);

            searchBox = new TextBox
            {
                Location = new Point(30, 125),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 10)
            };

            searchBox.TextChanged += SearchBox_TextChanged;

            Controls.Add(searchBox);

           
            // REFRESH BUTTON
           

            refreshButton = new Button
            {
                Text = "Refresh",
                Location = new Point(400, 123),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(0, 91, 170),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold)
            };

            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.Click += RefreshButton_Click;

            Controls.Add(refreshButton);

        
            // DATA GRID
      

            customersGrid = new DataGridView
            {
                Location = new Point(30, 180),
                Size = new Size(1040, 420),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Bottom |
                         AnchorStyles.Left |
                         AnchorStyles.Right,

                AutoGenerateColumns = true,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill
            };

            Controls.Add(customersGrid);
        }

        // LOAD CUSTOMERS
      

        private async void LoadCustomers()
        {
            try
            {
                refreshButton.Enabled = false;

                HttpResponseMessage response =
                    await _httpClient.GetAsync("api/Customers");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Unable to load customers from the API.",
                        "NWSDB",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                string json =
                    await response.Content.ReadAsStringAsync();

                var customers =
                    JsonSerializer.Deserialize<
                        List<CustomerDto>>(
                            json,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                customersGrid.DataSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading customers:\n\n" +
                    ex.Message,
                    "NWSDB",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                refreshButton.Enabled = true;
            }
        }


        // SEARCH
  

        private void SearchBox_TextChanged(
            object? sender,
            EventArgs e)
        {
            string search =
                searchBox.Text.Trim().ToLower();

            if (customersGrid.DataSource is not
                List<CustomerDto> customers)
            {
                return;
            }

            var filtered =
                customers.FindAll(c =>
                    c.AccountNumber
                        .ToLower()
                        .Contains(search)
                    ||
                    c.FullName
                        .ToLower()
                        .Contains(search)
                    ||
                    (c.NIC ?? "")
                        .ToLower()
                        .Contains(search));

            customersGrid.DataSource = filtered;
        }

     
        // REFRESH
      

        private void RefreshButton_Click(
            object? sender,
            EventArgs e)
        {
            searchBox.Clear();

            LoadCustomers();
        }
    }


    // CUSTOMER DTO


    public class CustomerDto
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }
            = string.Empty;

        public string FullName { get; set; }
            = string.Empty;

        public string? NIC { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string Status { get; set; }
            = string.Empty;
    }
}