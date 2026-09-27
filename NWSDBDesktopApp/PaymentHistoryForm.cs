using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using NWSDB.BusinessLogic.DTOs;

namespace NWSDBDesktopApp
{
    public partial class PaymentHistoryForm : Form
    {
        private readonly int _userId;
        private readonly HttpClient _httpClient;

        private Label customerNameLabel = null!;
        private Label accountNumberLabel = null!;
        private Label totalPaidValueLabel = null!;
        private Label paymentCountValueLabel = null!;
        private DataGridView paymentGrid = null!;

        public PaymentHistoryForm(int userId)
        {
            _userId = userId;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7293/")
            };

            BuildForm();

            Load += PaymentHistoryForm_Load;
        }

     
        // FORM LOAD
    

        private async void PaymentHistoryForm_Load(
            object? sender,
            EventArgs e)
        {
            await LoadPaymentHistoryAsync();
        }

        
        // BUILD COMPLETE FORM

        private void BuildForm()
        {
            Text = "NWSDB - Payment History";

            StartPosition =
                FormStartPosition.CenterScreen;

            ClientSize =
                new Size(1150, 720);

            MinimumSize =
                new Size(1000, 650);

            BackColor = Color.White;

            // MAIN TABLE

            TableLayoutPanel mainLayout =
                new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 2,

                    Margin = new Padding(0),
                    Padding = new Padding(0),

                    BackColor = Color.White
                };

            // Left sidebar
            mainLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    220));

            // Main content
            mainLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    100));

            // Header
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    80));

            // Body
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            // HEADER

            Panel header =
                new Panel
                {
                    Dock = DockStyle.Fill,

                    BackColor =
                        Color.FromArgb(
                            0,
                            83,
                            156),

                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };

            // NWSDB

            Label logoLabel =
                new Label
                {
                    Text = "NWSDB",

                    Font =
                        new Font(
                            "Segoe UI",
                            18,
                            FontStyle.Bold),

                    ForeColor = Color.White,

                    Location =
                        new Point(
                            25,
                            7),

                    Size =
                        new Size(
                            170,
                            32),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            // CUSTOMER PORTAL

            Label portalLabel =
                new Label
                {
                    Text = "Customer Portal",

                    Font =
                        new Font(
                            "Segoe UI",
                            9,
                            FontStyle.Regular),

                    ForeColor = Color.White,

                    Location =
                        new Point(
                            27,
                            43),

                    Size =
                        new Size(
                            170,
                            22),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            header.Controls.Add(logoLabel);
            header.Controls.Add(portalLabel);

            // -----------------------------------------------------
            // CUSTOMER DETAILS - RIGHT SIDE
            // -----------------------------------------------------

            customerNameLabel =
                new Label
                {
                    Text = "Customer",

                    Font =
                        new Font(
                            "Segoe UI",
                            10,
                            FontStyle.Bold),

                    ForeColor = Color.White,

                    Size =
                        new Size(
                            300,
                            25),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleRight
                };

            accountNumberLabel =
                new Label
                {
                    Text = "Account",

                    Font =
                        new Font(
                            "Segoe UI",
                            9),

                    ForeColor = Color.White,

                    Size =
                        new Size(
                            300,
                            22),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleRight
                };

            header.Controls.Add(
                customerNameLabel);

            header.Controls.Add(
                accountNumberLabel);

            // Position right-side labels
            Action positionHeaderLabels = () =>
            {
                customerNameLabel.Location =
                    new Point(
                        header.ClientSize.Width
                        - 330,
                        10);

                accountNumberLabel.Location =
                    new Point(
                        header.ClientSize.Width
                        - 330,
                        40);
            };

            header.Resize +=
                (s, e) =>
                {
                    positionHeaderLabels();
                };

            // Initial position
            positionHeaderLabels();

            // Add header
            mainLayout.Controls.Add(
                header,
                0,
                0);

            mainLayout.SetColumnSpan(
                header,
                2);

            // SIDEBAR

            Panel sidebar =
                new Panel
                {
                    Dock = DockStyle.Fill,

                    BackColor =
                        Color.FromArgb(
                            247,
                            249,
                            252),

                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };

            // MENU TITLE

            Label menuTitle =
                new Label
                {
                    Text = "CUSTOMER MENU",

                    Font =
                        new Font(
                            "Segoe UI",
                            9,
                            FontStyle.Bold),

                    ForeColor = Color.Gray,

                    Location =
                        new Point(
                            20,
                            20),

                    Size =
                        new Size(
                            180,
                            25),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            sidebar.Controls.Add(menuTitle);

            // MENU BUTTONS

            Button dashboardButton =
                CreateMenuButton(
                    "Dashboard",
                    55);

            Button currentBillButton =
                CreateMenuButton(
                    "Current Bill",
                    105);

            Button onlinePaymentButton =
                CreateMenuButton(
                    "Online Payment",
                    155);

            Button paymentHistoryButton =
                CreateMenuButton(
                    "Payment History",
                    205);

            Button waterUsageButton =
                CreateMenuButton(
                    "Water Usage",
                    255);

            Button profileButton =
                CreateMenuButton(
                    "Profile",
                    305);

            Button logoutButton =
                CreateMenuButton(
                    "Logout",
                    375);

            // Active item
            paymentHistoryButton.BackColor =
                Color.FromArgb(
                    220,
                    235,
                    250);

            paymentHistoryButton.ForeColor =
                Color.FromArgb(
                    0,
                    83,
                    156);

            // Events
            dashboardButton.Click +=
                DashboardButton_Click;

            onlinePaymentButton.Click +=
                OnlinePaymentButton_Click;

            profileButton.Click +=
                ProfileButton_Click;

            logoutButton.Click +=
                LogoutButton_Click;

            // Add buttons
            sidebar.Controls.Add(
                dashboardButton);

            sidebar.Controls.Add(
                currentBillButton);

            sidebar.Controls.Add(
                onlinePaymentButton);

            sidebar.Controls.Add(
                paymentHistoryButton);

            sidebar.Controls.Add(
                waterUsageButton);

            sidebar.Controls.Add(
                profileButton);

            sidebar.Controls.Add(
                logoutButton);

            mainLayout.Controls.Add(
                sidebar,
                0,
                1);

            
            // CONTENT

            Panel content =
                new Panel
                {
                    Dock = DockStyle.Fill,

                    BackColor = Color.White,

                    Margin = new Padding(0),

                    Padding =
                        new Padding(
                            30,
                            20,
                            30,
                            25),

                    AutoScroll = true
                };

            // PAGE TITLE

            Label pageTitle =
                new Label
                {
                    Text = "Payment History",

                    Font =
                        new Font(
                            "Segoe UI",
                            20,
                            FontStyle.Bold),

                    ForeColor =
                        Color.FromArgb(
                            35,
                            35,
                            35),

                    Location =
                        new Point(
                            30,
                            15),

                    Size =
                        new Size(
                            600,
                            42),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            content.Controls.Add(pageTitle);

            // PAGE SUBTITLE

            Label pageSubtitle =
                new Label
                {
                    Text =
                        "View your previous NWSDB payment transactions.",

                    Font =
                        new Font(
                            "Segoe UI",
                            10),

                    ForeColor = Color.Gray,

                    Location =
                        new Point(
                            32,
                            58),

                    Size =
                        new Size(
                            700,
                            25),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            content.Controls.Add(pageSubtitle);

            // TOTAL PAID CARD

            Panel totalCard =
                CreateSummaryCard(
                    "TOTAL PAID",
                    "LKR 0.00");

            totalCard.Location =
                new Point(
                    30,
                    100);

            totalPaidValueLabel =
                totalCard.Controls
                    .OfType<Label>()
                    .Last();

            content.Controls.Add(totalCard);

            // PAYMENT COUNT CARD

            Panel countCard =
                CreateSummaryCard(
                    "TOTAL PAYMENTS",
                    "0");

            countCard.Location =
                new Point(
                    270,
                    100);

            paymentCountValueLabel =
                countCard.Controls
                    .OfType<Label>()
                    .Last();

            content.Controls.Add(countCard);

            // TRANSACTION TITLE
         
            Label transactionTitle =
                new Label
                {
                    Text = "Transaction History",

                    Font =
                        new Font(
                            "Segoe UI",
                            14,
                            FontStyle.Bold),

                    ForeColor =
                        Color.FromArgb(
                            40,
                            40,
                            40),

                    Location =
                        new Point(
                            30,
                            205),

                    Size =
                        new Size(
                            400,
                            32),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            content.Controls.Add(
                transactionTitle);

          
            // PAYMENT TABLE
            

            paymentGrid =
                new DataGridView
                {
                    Location =
                        new Point(
                            30,
                            245),

                    Size =
                        new Size(
                            820,
                            370),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Bottom |
                        AnchorStyles.Left |
                        AnchorStyles.Right,

                    BackgroundColor =
                        Color.White,

                    BorderStyle =
                        BorderStyle.FixedSingle,

                    AllowUserToAddRows = false,

                    AllowUserToDeleteRows = false,

                    ReadOnly = true,

                    MultiSelect = false,

                    RowHeadersVisible = false,

                    SelectionMode =
                        DataGridViewSelectionMode.FullRowSelect,

                    AutoGenerateColumns = false,

                    AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill,

                    EnableHeadersVisualStyles = false,

                    CellBorderStyle =
                        DataGridViewCellBorderStyle.SingleHorizontal,

                    GridColor =
                        Color.FromArgb(
                            230,
                            230,
                            230)
                };

            // Header height
            paymentGrid.ColumnHeadersHeight = 42;

            // Row height
            paymentGrid.RowTemplate.Height = 38;

         
            // GRID HEADER
            

            paymentGrid.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(
                            0,
                            83,
                            156),

                    ForeColor = Color.White,

                    Font =
                        new Font(
                            "Segoe UI",
                            9,
                            FontStyle.Bold),

                    Alignment =
                        DataGridViewContentAlignment.MiddleLeft,

                    Padding =
                        new Padding(
                            8,
                            0,
                            8,
                            0)
                };

            // -----------------------------------------------------
            // GRID CELLS
            // -----------------------------------------------------

            paymentGrid.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    Font =
                        new Font(
                            "Segoe UI",
                            9),

                    Padding =
                        new Padding(
                            8,
                            0,
                            8,
                            0),

                    SelectionBackColor =
                        Color.FromArgb(
                            220,
                            235,
                            250),

                    SelectionForeColor =
                        Color.Black
                };

            paymentGrid.AlternatingRowsDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(
                            248,
                            250,
                            252)
                };

            // COLUMNS

            paymentGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Payment Reference",

                    DataPropertyName =
                        "PaymentReference",

                    FillWeight = 130
                });

            paymentGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Date",

                    DataPropertyName =
                        "PaymentDate",

                    FillWeight = 110
                });

            paymentGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Amount",

                    DataPropertyName =
                        "Amount",

                    FillWeight = 100
                });

            paymentGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Payment Method",

                    DataPropertyName =
                        "PaymentMethod",

                    FillWeight = 100
                });

            paymentGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText =
                        "Status",

                    DataPropertyName =
                        "Status",

                    FillWeight = 80
                });

            content.Controls.Add(
                paymentGrid);

            // ADD CONTENT

            mainLayout.Controls.Add(
                content,
                1,
                1);

            // ADD MAIN LAYOUT TO FORM

            Controls.Add(mainLayout);
        }

       
        // MENU BUTTON
        

        private Button CreateMenuButton(
            string text,
            int top)
        {
            Button button =
                new Button
                {
                    Text = text,

                    Location =
                        new Point(
                            10,
                            top),

                    Size =
                        new Size(
                            195,
                            42),

                    FlatStyle =
                        FlatStyle.Flat,

                    BackColor =
                        Color.Transparent,

                    ForeColor =
                        Color.FromArgb(
                            50,
                            50,
                            50),

                    Font =
                        new Font(
                            "Segoe UI",
                            10),

                    TextAlign =
                        ContentAlignment.MiddleLeft,

                    Padding =
                        new Padding(
                            15,
                            0,
                            0,
                            0),

                    Cursor =
                        Cursors.Hand
                };

            button.FlatAppearance.BorderSize = 0;

            return button;
        }

       
        // SUMMARY CARD
       

        private Panel CreateSummaryCard(
            string title,
            string value)
        {
            Panel card =
                new Panel
                {
                    Size =
                        new Size(
                            220,
                            90),

                    BackColor =
                        Color.FromArgb(
                            248,
                            250,
                            252),

                    BorderStyle =
                        BorderStyle.FixedSingle
                };

            Label titleLabel =
                new Label
                {
                    Text = title,

                    Font =
                        new Font(
                            "Segoe UI",
                            9,
                            FontStyle.Bold),

                    ForeColor = Color.Gray,

                    Location =
                        new Point(
                            15,
                            10),

                    Size =
                        new Size(
                            185,
                            22),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            Label valueLabel =
                new Label
                {
                    Text = value,

                    Font =
                        new Font(
                            "Segoe UI",
                            16,
                            FontStyle.Bold),

                    ForeColor =
                        Color.FromArgb(
                            0,
                            83,
                            156),

                    Location =
                        new Point(
                            15,
                            38),

                    Size =
                        new Size(
                            190,
                            35),

                    AutoSize = false,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);

            return card;
        }

     
        // LOAD PAYMENT HISTORY
     

        private async Task LoadPaymentHistoryAsync()
        {
            try
            {
                Cursor =
                    Cursors.WaitCursor;

                // Get customer

                CustomerDto? customer =
                    await _httpClient
                        .GetFromJsonAsync<CustomerDto>(
                            $"api/Customers/by-user/{_userId}");

                if (customer == null)
                {
                    MessageBox.Show(
                        "Customer details could not be found.",
                        "NWSDB",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // Header information

                customerNameLabel.Text =
                    customer.FullName;

                accountNumberLabel.Text =
                    $"Account: {customer.AccountNumber}";

                // Get customer payments

                List<PaymentDto>? payments =
                    await _httpClient
                        .GetFromJsonAsync<List<PaymentDto>>(
                            $"api/Payments/customer/{customer.Id}");

                payments ??=
                    new List<PaymentDto>();

                // Total paid

                decimal totalPaid =
                    payments.Sum(
                        p => p.Amount);

                totalPaidValueLabel.Text =
                    $"LKR {totalPaid:N2}";

                // Number of payments

                paymentCountValueLabel.Text =
                    payments.Count.ToString();

                // Prepare table data

                var displayPayments =
                    payments
                        .Select(
                            p => new
                            {
                                PaymentReference =
                                    p.PaymentReference,

                                PaymentDate =
                                    p.PaymentDate
                                        .ToLocalTime()
                                        .ToString(
                                            "dd MMM yyyy HH:mm"),

                                Amount =
                                    $"LKR {p.Amount:N2}",

                                PaymentMethod =
                                    p.PaymentMethod,

                                Status =
                                    p.Status
                            })
                        .ToList();

                // Display data

                paymentGrid.DataSource =
                    displayPayments;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                    "Could not connect to the NWSDB API.\n\n" +
                    ex.Message,

                    "Connection Error",

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load payment history.\n\n" +
                    ex.Message,

                    "NWSDB",

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor =
                    Cursors.Default;
            }
        }

        // DASHBOARD

        private void DashboardButton_Click(
            object? sender,
            EventArgs e)
        {
            CustomerDashboardForm dashboard =
                new CustomerDashboardForm(
                    _userId);

            dashboard.FormClosed +=
                (s, args) =>
                {
                    Show();
                };

            Hide();

            dashboard.Show();
        }

        // ONLINE PAYMENT

        private void OnlinePaymentButton_Click(
            object? sender,
            EventArgs e)
        {
            OnlinePaymentForm paymentForm =
                new OnlinePaymentForm(
                    _userId);

            paymentForm.FormClosed +=
                async (s, args) =>
                {
                    Show();

                    await LoadPaymentHistoryAsync();
                };

            Hide();

            paymentForm.Show();
        }

        // PROFILE

        private void ProfileButton_Click(
            object? sender,
            EventArgs e)
        {
            ProfileForm profileForm =
                new ProfileForm(
                    _userId);

            profileForm.FormClosed +=
                (s, args) =>
                {
                    Show();
                };

            Hide();

            profileForm.Show();
        }

        // LOGOUT

        private void LogoutButton_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to logout?",

                    "Logout",

                    MessageBoxButtons.YesNo,

                    MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        // FORM CLOSED

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            _httpClient.Dispose();

            base.OnFormClosed(e);
        }
    }
}

