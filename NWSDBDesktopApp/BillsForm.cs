using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NWSDBDesktopApp
{
    public partial class BillsForm : Form
    {
        private readonly HttpClient _httpClient;

        private DataGridView billsGrid = null!;
        private TextBox searchBox = null!;
        private Button refreshButton = null!;

        private List<BillDto> allBills = new();

        public BillsForm()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(
                    "http://localhost:5049/")
            };

            CreateBillsUI();

            LoadBills();
        }

        // CREATE BILLS UI

        private void CreateBillsUI()
        {
            Text = "NWSDB - Bills";

            StartPosition =
                FormStartPosition.CenterScreen;

            ClientSize =
                new Size(1150, 650);

            MinimumSize =
                new Size(950, 550);

            BackColor =
                Color.White;

            // HEADER

            Panel header = new Panel
            {
                Location =
                    new Point(0, 0),

                Size =
                    new Size(
                        ClientSize.Width,
                        70),

                BackColor =
                    Color.FromArgb(
                        0,
                        91,
                        170),

                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right
            };

            Controls.Add(header);

            Label title = new Label
            {
                Text =
                    "Bill Management",

                Location =
                    new Point(
                        25,
                        18),

                AutoSize = true,

                ForeColor =
                    Color.White,

                Font =
                    new Font(
                        "Segoe UI",
                        19,
                        FontStyle.Bold)
            };

            header.Controls.Add(title);

            // SEARCH LABEL

            Label searchLabel = new Label
            {
                Text =
                    "Search Bill",

                Location =
                    new Point(
                        30,
                        100),

                AutoSize = true,

                Font =
                    new Font(
                        "Segoe UI",
                        10,
                        FontStyle.Bold)
            };

            Controls.Add(searchLabel);

            // SEARCH BOX

            searchBox = new TextBox
            {
                Location =
                    new Point(
                        30,
                        125),

                Size =
                    new Size(
                        350,
                        30),

                Font =
                    new Font(
                        "Segoe UI",
                        10)
            };

            searchBox.TextChanged +=
                SearchBox_TextChanged;

            Controls.Add(searchBox);

            // REFRESH BUTTON

            refreshButton = new Button
            {
                Text =
                    "Refresh",

                Location =
                    new Point(
                        400,
                        123),

                Size =
                    new Size(
                        100,
                        35),

                BackColor =
                    Color.FromArgb(
                        0,
                        91,
                        170),

                ForeColor =
                    Color.White,

                FlatStyle =
                    FlatStyle.Flat,

                Font =
                    new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold),

                Cursor =
                    Cursors.Hand
            };

            refreshButton
                .FlatAppearance
                .BorderSize = 0;

            refreshButton.Click +=
                RefreshButton_Click;

            Controls.Add(
                refreshButton);

            // BILLS GRID

            billsGrid = new DataGridView
            {
                Location =
                    new Point(
                        30,
                        180),

                Size =
                    new Size(
                        1090,
                        420),

                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Bottom |
                    AnchorStyles.Left |
                    AnchorStyles.Right,

                AutoGenerateColumns = false,

                AllowUserToAddRows = false,

                ReadOnly = true,

                SelectionMode =
                    DataGridViewSelectionMode
                        .FullRowSelect,

                MultiSelect = false,

                BackgroundColor =
                    Color.White,

                BorderStyle =
                    BorderStyle.FixedSingle,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode
                        .Fill
            };

            // COLUMNS

            billsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Bill Number",
                    DataPropertyName =
                        "BillNumber"
                });

            billsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Customer",
                    DataPropertyName =
                        "CustomerName"
                });

            billsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Billing Period",
                    DataPropertyName =
                        "BillingPeriod"
                });

            billsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Issue Date",
                    DataPropertyName =
                        "IssueDate"
                });

            billsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Due Date",
                    DataPropertyName =
                        "DueDate"
                });

            billsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Units",
                    DataPropertyName =
                        "UnitsConsumed"
                });

            billsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Total Amount",
                    DataPropertyName =
                        "TotalAmount"
                });

            billsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Status",
                    DataPropertyName =
                        "Status"
                });

            Controls.Add(
                billsGrid);
        }

        // LOAD BILLS

        private async void LoadBills()
        {
            try
            {
                refreshButton.Enabled = false;

                HttpResponseMessage response =
                    await _httpClient.GetAsync(
                        "api/Bills");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Unable to load bills from the API.",
                        "NWSDB",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                string json =
                    await response.Content
                        .ReadAsStringAsync();

                var bills =
                    JsonSerializer.Deserialize<
                        List<BillDto>>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive =
                                true
                        });

                allBills =
                    bills ?? new List<BillDto>();

                DisplayBills(allBills);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading bills:\n\n" +
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

        // DISPLAY BILLS

        private void DisplayBills(
            List<BillDto> bills)
        {
            billsGrid.DataSource = null;

            billsGrid.DataSource = bills;

            // Format dates
            if (billsGrid.Columns.Contains(
                "IssueDate"))
            {
                billsGrid.Columns[
                    "IssueDate"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy";
            }

            if (billsGrid.Columns.Contains(
                "DueDate"))
            {
                billsGrid.Columns[
                    "DueDate"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy";
            }

            // Format amount
            if (billsGrid.Columns.Contains(
                "TotalAmount"))
            {
                billsGrid.Columns[
                    "TotalAmount"]
                    .DefaultCellStyle.Format =
                    "N2";
            }
        }

        // SEARCH

        private void SearchBox_TextChanged(
            object? sender,
            EventArgs e)
        {
            string search =
                searchBox.Text
                    .Trim()
                    .ToLower();

            if (string.IsNullOrWhiteSpace(
                search))
            {
                DisplayBills(allBills);
                return;
            }

            List<BillDto> filtered =
                allBills.FindAll(
                    bill =>
                        bill.BillNumber
                            .ToLower()
                            .Contains(search)
                        ||
                        bill.CustomerName
                            .ToLower()
                            .Contains(search)
                        ||
                        bill.BillingPeriod
                            .ToLower()
                            .Contains(search)
                        ||
                        bill.Status
                            .ToLower()
                            .Contains(search));

            DisplayBills(filtered);
        }

        // REFRESH

        private void RefreshButton_Click(
            object? sender,
            EventArgs e)
        {
            searchBox.Clear();

            LoadBills();
        }
    }

    // BILL DTO

    public class BillDto
    {
        public int Id { get; set; }

        public string BillNumber { get; set; }
            = string.Empty;

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }
            = string.Empty;

        public string BillingPeriod { get; set; }
            = string.Empty;

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public int UnitsConsumed { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }
            = string.Empty;
    }
}