using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NWSDBDesktopApp
{
    public partial class PaymentsForm : Form
    {
        private readonly HttpClient _httpClient;

        private DataGridView paymentsGrid = null!;
        private TextBox searchBox = null!;
        private Button refreshButton = null!;

        private List<PaymentDto> allPayments = new();

        public PaymentsForm()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(
                    "http://localhost:5049/")
            };

            CreatePaymentsUI();

            LoadPayments();
        }

    
        // BUILD PAYMENTS UI

        private void CreatePaymentsUI()
        {
            Text = "NWSDB - Payments";

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
                    "Payment Management",

                Location =
                    new Point(25, 18),

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
                    "Search Payment",

                Location =
                    new Point(30, 100),

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
                    new Point(30, 125),

                Size =
                    new Size(350, 30),

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
                    new Point(400, 123),

                Size =
                    new Size(100, 35),

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

            refreshButton.FlatAppearance
                .BorderSize = 0;

            refreshButton.Click +=
                RefreshButton_Click;

            Controls.Add(refreshButton);

            // PAYMENTS GRID

            paymentsGrid = new DataGridView
            {
                Location =
                    new Point(30, 180),

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
                    DataGridViewSelectionMode.FullRowSelect,

                MultiSelect = false,

                BackgroundColor =
                    Color.White,

                BorderStyle =
                    BorderStyle.FixedSingle,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill
            };

            // COLUMNS

            paymentsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Payment Reference",

                    DataPropertyName =
                        "PaymentReference"
                });

            paymentsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Customer ID",

                    DataPropertyName =
                        "CustomerId"
                });

            paymentsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Bill ID",

                    DataPropertyName =
                        "BillId"
                });

            paymentsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Payment Date",

                    DataPropertyName =
                        "PaymentDate"
                });

            paymentsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Amount (LKR)",

                    DataPropertyName =
                        "Amount"
                });

            paymentsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Payment Method",

                    DataPropertyName =
                        "PaymentMethod"
                });

            paymentsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Status",

                    DataPropertyName =
                        "Status"
                });

            Controls.Add(paymentsGrid);
        }

        // LOAD PAYMENTS

        private async void LoadPayments()
        {
            try
            {
                refreshButton.Enabled = false;

                HttpResponseMessage response =
                    await _httpClient.GetAsync(
                        "api/Payments");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Unable to load payments from the API.",
                        "NWSDB",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                string json =
                    await response.Content
                        .ReadAsStringAsync();

                var payments =
                    JsonSerializer.Deserialize<
                        List<PaymentDto>>(
                        json,

                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive =
                                true
                        });

                allPayments =
                    payments ??
                    new List<PaymentDto>();

                DisplayPayments(
                    allPayments);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading payments:\n\n" +
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

        // DISPLAY PAYMENTS

        private void DisplayPayments(
            List<PaymentDto> payments)
        {
            paymentsGrid.DataSource = null;

            paymentsGrid.DataSource =
                payments;

            if (
                paymentsGrid.Columns.Contains(
                    "PaymentDate"))
            {
                paymentsGrid.Columns[
                    "PaymentDate"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }

            if (
                paymentsGrid.Columns.Contains(
                    "Amount"))
            {
                paymentsGrid.Columns[
                    "Amount"]
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

            if (
                string.IsNullOrWhiteSpace(
                    search))
            {
                DisplayPayments(
                    allPayments);

                return;
            }

            List<PaymentDto> filtered =
                allPayments.FindAll(
                    payment =>
                        payment.PaymentReference
                            .ToLower()
                            .Contains(search)
                        ||
                        payment.CustomerId
                            .ToString()
                            .Contains(search)
                        ||
                        payment.BillId
                            .ToString()
                            .Contains(search)
                        ||
                        payment.PaymentMethod
                            .ToLower()
                            .Contains(search)
                        ||
                        payment.Status
                            .ToLower()
                            .Contains(search));

            DisplayPayments(
                filtered);
        }

        // REFRESH

        private void RefreshButton_Click(
            object? sender,
            EventArgs e)
        {
            searchBox.Clear();

            LoadPayments();
        }

        private void PaymentsForm_Load(object sender, EventArgs e)
        {

        }
    }

    // PAYMENT DTO

    public class PaymentDto
    {
        public int Id { get; set; }

        public string PaymentReference { get; set; }
            = string.Empty;

        public int CustomerId { get; set; }

        public int? BillId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }
            = string.Empty;

        public string Status { get; set; }
            = string.Empty;
    }
}