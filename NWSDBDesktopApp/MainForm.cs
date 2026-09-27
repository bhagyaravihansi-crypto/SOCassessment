using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NWSDBDesktopApp
{
    public partial class MainForm : Form
    {
        private readonly HttpClient _httpClient;

        private Label customerCountLabel = null!;
        private Label pendingBillCountLabel = null!;
        private Label paymentCountLabel = null!;

        public MainForm()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5049/")
            };

            BuildDashboard();
            LoadDashboardData();
        }

     
        // BUILD DASHBOARD
      
        private void BuildDashboard()
        {
            Text = "NWSDB Desktop Application";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1100, 650);
            MinimumSize = new Size(900, 550);
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
                Text = "NWSDB Desktop Application",
                Location = new Point(25, 17),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font(
                    "Segoe UI",
                    19,
                    FontStyle.Bold)
            };

            header.Controls.Add(title);

            Label admin = new Label
            {
                Text = "Administrator",
                Location = new Point(900, 25),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font(
                    "Segoe UI",
                    10)
            };

            header.Controls.Add(admin);

           
            // SIDEBAR
           

            Panel sidebar = new Panel
            {
                Location = new Point(0, 70),
                Size = new Size(
                    220,
                    ClientSize.Height - 70),
                BackColor = Color.FromArgb(
                    245,
                    248,
                    252),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Bottom |
                         AnchorStyles.Left
            };

            Controls.Add(sidebar);

            Label menuTitle = new Label
            {
                Text = "MAIN MENU",
                Location = new Point(25, 25),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold)
            };

            sidebar.Controls.Add(menuTitle);

         
            // DASHBOARD BUTTON
          

            Button dashboardButton =
                CreateMenuButton("Dashboard");

            dashboardButton.Location =
                new Point(15, 60);

            sidebar.Controls.Add(
                dashboardButton);

        
            // CUSTOMERS BUTTON
         

            Button customersButton =
                CreateMenuButton("Customers");

            customersButton.Location =
                new Point(15, 115);

            customersButton.Click +=
                Customers_Click;

            sidebar.Controls.Add(
                customersButton);
  
            // BILLS BUTTON
           

            Button billsButton =
                CreateMenuButton("Bills");

            billsButton.Location =
                new Point(15, 170);

            billsButton.Click +=
                Bills_Click;

            sidebar.Controls.Add(
                billsButton);

            
            // PAYMENTS BUTTON
            

            Button paymentsButton =
                CreateMenuButton("Payments");

            paymentsButton.Location =
                new Point(15, 225);

            paymentsButton.Click +=
                Payments_Click;

            sidebar.Controls.Add(
                paymentsButton);

          
            // LOGOUT BUTTON
         

            Button logoutButton =
                CreateMenuButton("Logout");

            logoutButton.Location =
                new Point(
                    15,
                    sidebar.Height - 65);

            logoutButton.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            logoutButton.Click +=
                Logout_Click;

            sidebar.Controls.Add(
                logoutButton);

        
            // CONTENT AREA

            Panel content = new Panel
            {
                Location = new Point(220, 70),

                Size = new Size(
                    ClientSize.Width - 220,
                    ClientSize.Height - 70),

                BackColor = Color.White,

                Anchor = AnchorStyles.Top |
                         AnchorStyles.Bottom |
                         AnchorStyles.Left |
                         AnchorStyles.Right,

                AutoScroll = true
            };

            Controls.Add(content);

            // DASHBOARD TITLE

            Label dashboardTitle = new Label
            {
                Text = "Dashboard",

                Location =
                    new Point(30, 30),

                AutoSize = true,

                ForeColor =
                    Color.FromArgb(
                        30,
                        50,
                        80),

                Font =
                    new Font(
                        "Segoe UI",
                        20,
                        FontStyle.Bold)
            };

            content.Controls.Add(
                dashboardTitle);

            // SUBTITLE

            Label subtitle = new Label
            {
                Text =
                    "National Water Supply and Drainage Board",

                Location =
                    new Point(33, 75),

                AutoSize = true,

                ForeColor =
                    Color.Gray,

                Font =
                    new Font(
                        "Segoe UI",
                        10)
            };

            content.Controls.Add(
                subtitle);

            // CUSTOMER CARD

            Panel customerCard =
                CreateCard(
                    "Customers",
                    "Loading...");

            customerCard.Location =
                new Point(30, 125);

            content.Controls.Add(
                customerCard);

            customerCountLabel =
                GetCardValueLabel(
                    customerCard);

            // PENDING BILL CARD

            Panel billCard =
                CreateCard(
                    "Pending Bills",
                    "Loading...");

            billCard.Location =
                new Point(265, 125);

            content.Controls.Add(
                billCard);

            pendingBillCountLabel =
                GetCardValueLabel(
                    billCard);

            // PAYMENT CARD

            Panel paymentCard =
                CreateCard(
                    "Payments",
                    "Loading...");

            paymentCard.Location =
                new Point(500, 125);

            content.Controls.Add(
                paymentCard);

            paymentCountLabel =
                GetCardValueLabel(
                    paymentCard);

            // SYSTEM OVERVIEW

            Panel overview = new Panel
            {
                Location =
                    new Point(30, 275),

                Size =
                    new Size(680, 180),

                BorderStyle =
                    BorderStyle.FixedSingle,

                BackColor =
                    Color.White
            };

            content.Controls.Add(
                overview);

            Label overviewTitle = new Label
            {
                Text =
                    "System Overview",

                Location =
                    new Point(20, 20),

                AutoSize = true,

                Font =
                    new Font(
                        "Segoe UI",
                        14,
                        FontStyle.Bold),

                ForeColor =
                    Color.FromArgb(
                        30,
                        50,
                        80)
            };

            overview.Controls.Add(
                overviewTitle);

            Label overviewText = new Label
            {
                Text =
                    "Manage NWSDB customer accounts, bills and payments.\r\n\r\n" +
                    "Dashboard statistics are loaded from the NWSDB database through the API.",

                Location =
                    new Point(20, 60),

                AutoSize = true,

                Font =
                    new Font(
                        "Segoe UI",
                        10),

                ForeColor =
                    Color.Gray
            };

            overview.Controls.Add(
                overviewText);
        }

        
        // CREATE MENU BUTTON

        private Button CreateMenuButton(string text)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(190, 45),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(
                    30,
                    50,
                    80),
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold),
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

        // CREATE DASHBOARD CARD

        private Panel CreateCard(
            string title,
            string value)
        {
            Panel card = new Panel
            {
                Size =
                    new Size(
                        210,
                        120),

                BorderStyle =
                    BorderStyle.FixedSingle,

                BackColor =
                    Color.White
            };

            Label titleLabel = new Label
            {
                Text = title,

                Location =
                    new Point(
                        18,
                        18),

                AutoSize = true,

                ForeColor =
                    Color.Gray,

                Font =
                    new Font(
                        "Segoe UI",
                        10,
                        FontStyle.Bold)
            };

            card.Controls.Add(
                titleLabel);

            Label valueLabel = new Label
            {
                Text = value,

                Location =
                    new Point(
                        18,
                        48),

                AutoSize = true,

                ForeColor =
                    Color.FromArgb(
                        0,
                        91,
                        170),

                Font =
                    new Font(
                        "Segoe UI",
                        24,
                        FontStyle.Bold),

                Tag = "CardValue"
            };

            card.Controls.Add(
                valueLabel);

            return card;
        }

        // GET CARD VALUE LABEL

        private Label GetCardValueLabel(
            Panel card)
        {
            foreach (Control control in card.Controls)
            {
                if (control is Label label &&
                    label.Tag?.ToString() ==
                    "CardValue")
                {
                    return label;
                }
            }

            throw new InvalidOperationException(
                "Card value label was not found.");
        }

        // LOAD DASHBOARD DATA

        private async void LoadDashboardData()
        {
            try
            {
                await LoadCustomerCount();

                await LoadPendingBillCount();

                await LoadPaymentCount();
            }
            catch (Exception ex)
            {
                customerCountLabel.Text = "-";

                pendingBillCountLabel.Text = "-";

                paymentCountLabel.Text = "-";

                MessageBox.Show(
                    "Unable to load dashboard data.\n\n" +
                    "Please make sure NWSDB.API is running.\n\n" +
                    ex.Message,

                    "NWSDB",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Warning);
            }
        }

        // CUSTOMER COUNT

        private async Task LoadCustomerCount()
        {
            HttpResponseMessage response =
                await _httpClient.GetAsync(
                    "api/Customers");

            if (!response.IsSuccessStatusCode)
            {
                customerCountLabel.Text = "-";
                return;
            }

            string json =
                await response.Content
                    .ReadAsStringAsync();

            var customers =
                JsonSerializer.Deserialize<
                    List<CustomerDto>>(
                    json,

                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive =
                            true
                    });

            customerCountLabel.Text =
                (customers?.Count ?? 0)
                    .ToString();
        }

        // PENDING BILL COUNT

        private async Task LoadPendingBillCount()
        {
            HttpResponseMessage response =
                await _httpClient.GetAsync(
                    "api/Bills");

            if (!response.IsSuccessStatusCode)
            {
                pendingBillCountLabel.Text = "-";
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

            int pendingCount = 0;

            if (bills != null)
            {
                foreach (var bill in bills)
                {
                    if (
                        bill.Status.Equals(
                            "Pending",
                            StringComparison
                                .OrdinalIgnoreCase)
                        ||
                        bill.Status.Equals(
                            "Unpaid",
                            StringComparison
                                .OrdinalIgnoreCase))
                    {
                        pendingCount++;
                    }
                }
            }

            pendingBillCountLabel.Text =
                pendingCount.ToString();
        }

        // PAYMENT COUNT

        private async Task LoadPaymentCount()
        {
            HttpResponseMessage response =
                await _httpClient.GetAsync(
                    "api/Payments");

            if (!response.IsSuccessStatusCode)
            {
                paymentCountLabel.Text = "-";
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

            paymentCountLabel.Text =
                (payments?.Count ?? 0)
                    .ToString();
        }

        // CUSTOMERS BUTTON CLICK

        private void Customers_Click(
            object? sender,
            EventArgs e)
        {
            CustomersForm customersForm =
                new CustomersForm();

            customersForm.ShowDialog();
        }

        // BILLS BUTTON CLICK

        private void Bills_Click(
            object? sender,
            EventArgs e)
        {
            BillsForm billsForm =
                new BillsForm();

            billsForm.ShowDialog();
        }

        // PAYMENTS BUTTON CLICK

        private void Payments_Click(
            object? sender,
            EventArgs e)
        {
            PaymentsForm paymentsForm =
                new PaymentsForm();

            paymentsForm.ShowDialog();
        }

        // LOGOUT

        private void Logout_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to logout?",

                    "NWSDB Logout",

                    MessageBoxButtons.YesNo,

                    MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}