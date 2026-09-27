using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;
using NWSDB.BusinessLogic.DTOs;

namespace NWSDBDesktopApp
{
    public partial class CustomerDashboardForm : Form
    {
        private readonly int _userId;
        private readonly HttpClient _httpClient;

        // Customer information labels
        private Label customerNameLabel = null!;
        private Label accountNumberLabel = null!;
        private Label emailLabel = null!;
        private Label phoneLabel = null!;
        private Label statusLabel = null!;

        // Dashboard summary labels
        private Label currentBillValueLabel = null!;
        private Label lastPaymentValueLabel = null!;
        private Label paymentStatusValueLabel = null!;

        public CustomerDashboardForm(int userId)
        {
            InitializeComponent();

            _userId = userId;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7293/")
            };

            BuildDashboard();

            // Load actual customer data
            LoadCustomerDataAsync();
        }

        private void BuildDashboard()
        {
            Text = "NWSDB Customer Dashboard";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1100, 650);
            MinimumSize = new Size(900, 550);
            BackColor = Color.White;

            // =========================
            // HEADER
            // =========================

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
                Text = "NWSDB Customer Portal",
                Location = new Point(25, 17),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font(
                    "Segoe UI",
                    19,
                    FontStyle.Bold)
            };

            header.Controls.Add(title);

            Label accountLabel = new Label
            {
                Text = "Customer Account",
                Location = new Point(900, 25),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font(
                    "Segoe UI",
                    10)
            };

            header.Controls.Add(accountLabel);

            // =========================
            // SIDEBAR
            // =========================

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
                Text = "CUSTOMER MENU",
                Location = new Point(25, 25),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold)
            };

            sidebar.Controls.Add(menuTitle);

            // =========================
            // DASHBOARD BUTTON
            // =========================

            Button dashboardButton =
                CreateMenuButton("Dashboard");

            dashboardButton.Location =
                new Point(15, 60);

            sidebar.Controls.Add(
                dashboardButton);

            // =========================
            // CURRENT BILL BUTTON
            // =========================

            Button billButton =
                CreateMenuButton("Current Bill");

            billButton.Location =
                new Point(15, 115);

            billButton.Click +=
                CurrentBill_Click;

            sidebar.Controls.Add(
                billButton);

            // =========================
            // ONLINE PAYMENT BUTTON
            // =========================

            Button paymentButton =
                CreateMenuButton("Online Payment");

            paymentButton.Location =
                new Point(15, 170);

            paymentButton.Click +=
                OnlinePayment_Click;

            sidebar.Controls.Add(
                paymentButton);

            // =========================
            // PAYMENT HISTORY BUTTON
            // =========================

            Button historyButton =
                CreateMenuButton("Payment History");

            historyButton.Location =
                new Point(15, 225);

            historyButton.Click +=
                PaymentHistory_Click;

            sidebar.Controls.Add(
                historyButton);

            // =========================
            // WATER USAGE BUTTON
            // =========================

            Button usageButton =
                CreateMenuButton("Water Usage");

            usageButton.Location =
                new Point(15, 280);

            usageButton.Click +=
                WaterUsage_Click;

            sidebar.Controls.Add(
                usageButton);

            // =========================
            // PROFILE BUTTON
            // =========================

            Button profileButton =
                CreateMenuButton("Profile");

            profileButton.Location =
                new Point(15, 335);

            profileButton.Click +=
                Profile_Click;

            sidebar.Controls.Add(
                profileButton);

            // =========================
            // LOGOUT BUTTON
            // =========================

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

            // =========================
            // MAIN CONTENT
            // =========================

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

            // =========================
            // DASHBOARD TITLE
            // =========================

            Label dashboardTitle = new Label
            {
                Text = "Customer Dashboard",
                Location = new Point(30, 30),
                AutoSize = true,
                ForeColor = Color.FromArgb(
                    30,
                    50,
                    80),
                Font = new Font(
                    "Segoe UI",
                    20,
                    FontStyle.Bold)
            };

            content.Controls.Add(
                dashboardTitle);

            // =========================
            // WELCOME MESSAGE
            // =========================

            Label welcomeLabel = new Label
            {
                Text = "Welcome to the NWSDB Customer Portal",
                Location = new Point(33, 75),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    10)
            };

            content.Controls.Add(
                welcomeLabel);

            // =========================
            // SUMMARY CARDS
            // =========================

            Panel billCard =
                CreateCard(
                    "Current Bill",
                    "Loading...",
                    out currentBillValueLabel);

            billCard.Location =
                new Point(30, 125);

            content.Controls.Add(
                billCard);

            Panel paymentCard =
                CreateCard(
                    "Last Payment",
                    "Loading...",
                    out lastPaymentValueLabel);

            paymentCard.Location =
                new Point(265, 125);

            content.Controls.Add(
                paymentCard);

            Panel statusCard =
                CreateCard(
                    "Payment Status",
                    "Loading...",
                    out paymentStatusValueLabel);

            statusCard.Location =
                new Point(500, 125);

            content.Controls.Add(
                statusCard);

            // =========================
            // ACCOUNT INFORMATION
            // =========================

            Panel accountPanel = new Panel
            {
                Location = new Point(30, 275),
                Size = new Size(680, 210),
                BorderStyle =
                    BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            content.Controls.Add(
                accountPanel);

            Label accountTitle = new Label
            {
                Text = "Account Information",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    14,
                    FontStyle.Bold),
                ForeColor = Color.FromArgb(
                    30,
                    50,
                    80)
            };

            accountPanel.Controls.Add(
                accountTitle);

            // Customer Name

            customerNameLabel = new Label
            {
                Text = "Customer Name: Loading...",
                Location = new Point(20, 65),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10),
                ForeColor = Color.DimGray
            };

            accountPanel.Controls.Add(
                customerNameLabel);

            // Account Number

            accountNumberLabel = new Label
            {
                Text = "Account Number: Loading...",
                Location = new Point(20, 95),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10),
                ForeColor = Color.DimGray
            };

            accountPanel.Controls.Add(
                accountNumberLabel);

            // Email

            emailLabel = new Label
            {
                Text = "Email: Loading...",
                Location = new Point(20, 125),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10),
                ForeColor = Color.DimGray
            };

            accountPanel.Controls.Add(
                emailLabel);

            // Phone

            phoneLabel = new Label
            {
                Text = "Phone: Loading...",
                Location = new Point(20, 155),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10),
                ForeColor = Color.DimGray
            };

            accountPanel.Controls.Add(
                phoneLabel);

            // User ID

            Label userIdLabel = new Label
            {
                Text = "User ID: " + _userId,
                Location = new Point(350, 65),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    9),
                ForeColor = Color.Gray
            };

            accountPanel.Controls.Add(
                userIdLabel);

            // Status

            statusLabel = new Label
            {
                Text = "Status: Loading...",
                Location = new Point(350, 95),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold),
                ForeColor = Color.Gray
            };

            accountPanel.Controls.Add(
                statusLabel);

            // =========================
            // QUICK ACTIONS
            // =========================

            Label quickTitle = new Label
            {
                Text = "Quick Actions",
                Location = new Point(30, 505),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    14,
                    FontStyle.Bold),
                ForeColor = Color.FromArgb(
                    30,
                    50,
                    80)
            };

            content.Controls.Add(
                quickTitle);

            Button payButton = new Button
            {
                Text = "Pay Current Bill",
                Location = new Point(30, 545),
                Size = new Size(200, 45),
                BackColor = Color.FromArgb(
                    0,
                    91,
                    170),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            payButton.FlatAppearance.BorderSize = 0;

            payButton.Click +=
                OnlinePayment_Click;

            content.Controls.Add(
                payButton);

            Button historyActionButton = new Button
            {
                Text = "View Payment History",
                Location = new Point(245, 545),
                Size = new Size(200, 45),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(
                    0,
                    91,
                    170),
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            historyActionButton.FlatAppearance.BorderColor =
                Color.FromArgb(0, 91, 170);

            historyActionButton.Click +=
                PaymentHistory_Click;

            content.Controls.Add(
                historyActionButton);
        }

        // =========================
        // LOAD CUSTOMER DATA
        // =========================

        private async void LoadCustomerDataAsync()
        {
            try
            {
                CustomerDto? customer =
                    await _httpClient.GetFromJsonAsync<CustomerDto>(
                        $"api/Customers/by-user/{_userId}");

                if (customer == null)
                {
                    ShowCustomerNotFound();
                    return;
                }

                customerNameLabel.Text =
                    "Customer Name: " +
                    GetValue(customer.FullName);

                accountNumberLabel.Text =
                    "Account Number: " +
                    GetValue(customer.AccountNumber);

                emailLabel.Text =
                    "Email: " +
                    GetValue(customer.Email);

                phoneLabel.Text =
                    "Phone: " +
                    GetValue(customer.Phone);

                statusLabel.Text =
                    "Status: " +
                    GetValue(customer.Status);

                statusLabel.ForeColor =
                    customer.Status?.Equals(
                        "Active",
                        StringComparison.OrdinalIgnoreCase) == true
                        ? Color.Green
                        : Color.Red;

                // Load dashboard summary
                await LoadDashboardSummaryAsync(
                    customer.Id);
            }
            catch (HttpRequestException)
            {
                ShowSummaryUnavailable();

                MessageBox.Show(
                    "Unable to connect to NWSDB API.\n\n" +
                    "Please make sure the NWSDB.API project is running.",
                    "NWSDB Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                ShowSummaryUnavailable();

                MessageBox.Show(
                    "Unable to load customer information.\n\n" +
                    ex.Message,
                    "NWSDB Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================
        // LOAD DASHBOARD SUMMARY
        // =========================

        private async System.Threading.Tasks.Task
            LoadDashboardSummaryAsync(int customerId)
        {
            try
            {
                // =========================
                // LOAD CURRENT BILL
                // =========================

                List<BillDto>? bills =
                    await _httpClient.GetFromJsonAsync<List<BillDto>>(
                        $"api/Bills/customer/{customerId}");

                if (bills != null && bills.Count > 0)
                {
                    BillDto? latestBill =
                        bills
                            .OrderByDescending(
                                b => b.IssueDate)
                            .FirstOrDefault();

                    if (latestBill != null)
                    {
                        currentBillValueLabel.Text =
                            "LKR " +
                            latestBill.TotalAmount
                                .ToString("N2");
                    }
                    else
                    {
                        currentBillValueLabel.Text =
                            "No Bill";
                    }
                }
                else
                {
                    currentBillValueLabel.Text =
                        "No Bill";
                }

                // =========================
                // LOAD PAYMENTS
                // =========================

                List<PaymentDto>? payments =
                    await _httpClient.GetFromJsonAsync<List<PaymentDto>>(
                        $"api/Payments/customer/{customerId}");

                if (payments != null && payments.Count > 0)
                {
                    PaymentDto? latestPayment =
                        payments
                            .OrderByDescending(
                                p => p.PaymentDate)
                            .FirstOrDefault();

                    if (latestPayment != null)
                    {
                        lastPaymentValueLabel.Text =
                            "LKR " +
                            latestPayment.Amount
                                .ToString("N2");

                        paymentStatusValueLabel.Text =
                            GetValue(
                                latestPayment.Status);

                        if (latestPayment.Status?.Equals(
                                "Successful",
                                StringComparison.OrdinalIgnoreCase)
                            == true)
                        {
                            paymentStatusValueLabel.ForeColor =
                                Color.Green;
                        }
                        else
                        {
                            paymentStatusValueLabel.ForeColor =
                                Color.Red;
                        }
                    }
                }
                else
                {
                    lastPaymentValueLabel.Text =
                        "No Payment";

                    paymentStatusValueLabel.Text =
                        "No Payment";

                    paymentStatusValueLabel.ForeColor =
                        Color.Gray;
                }
            }
            catch (HttpRequestException)
            {
                ShowSummaryUnavailable();
            }
            catch (Exception)
            {
                ShowSummaryUnavailable();
            }
        }

        // =========================
        // SUMMARY UNAVAILABLE
        // =========================

        private void ShowSummaryUnavailable()
        {
            if (currentBillValueLabel != null)
            {
                currentBillValueLabel.Text =
                    "Unavailable";
            }

            if (lastPaymentValueLabel != null)
            {
                lastPaymentValueLabel.Text =
                    "Unavailable";
            }

            if (paymentStatusValueLabel != null)
            {
                paymentStatusValueLabel.Text =
                    "Unavailable";

                paymentStatusValueLabel.ForeColor =
                    Color.Gray;
            }
        }

        // =========================
        // CUSTOMER NOT FOUND
        // =========================

        private void ShowCustomerNotFound()
        {
            customerNameLabel.Text =
                "Customer Name: Not found";

            accountNumberLabel.Text =
                "Account Number: Not found";

            emailLabel.Text =
                "Email: Not found";

            phoneLabel.Text =
                "Phone: Not found";

            statusLabel.Text =
                "Status: Not found";

            statusLabel.ForeColor =
                Color.Red;

            ShowSummaryUnavailable();
        }

        // =========================
        // GET VALUE
        // =========================

        private string GetValue(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? "Not available"
                : value;
        }

        // =========================
        // MENU BUTTON
        // =========================

        private Button CreateMenuButton(
            string text)
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
                    new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;

            return button;
        }

        // =========================
        // DASHBOARD CARD
        // =========================

        private Panel CreateCard(
            string title,
            string value,
            out Label valueLabel)
        {
            Panel card = new Panel
            {
                Size = new Size(210, 120),
                BorderStyle =
                    BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            Label titleLabel = new Label
            {
                Text = title,
                Location = new Point(18, 18),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold)
            };

            card.Controls.Add(
                titleLabel);

            valueLabel = new Label
            {
                Text = value,
                Location = new Point(18, 48),
                AutoSize = true,
                ForeColor = Color.FromArgb(
                    0,
                    91,
                    170),
                Font = new Font(
                    "Segoe UI",
                    17,
                    FontStyle.Bold)
            };

            card.Controls.Add(
                valueLabel);

            return card;
        }

        // =========================
        // CURRENT BILL
        // =========================

        private void CurrentBill_Click(
            object? sender,
            EventArgs e)
        {
            MessageBox.Show(
                "Current Bill page will be connected next.",
                "NWSDB",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =========================
        // ONLINE PAYMENT
        // =========================

        private void OnlinePayment_Click(
            object? sender,
            EventArgs e)
        {
            OnlinePaymentForm paymentForm =
                new OnlinePaymentForm(_userId);

            paymentForm.FormClosed +=
                (s, args) => Show();

            Hide();

            paymentForm.Show();
        }

        // =========================
        // PAYMENT HISTORY
        // =========================

        private void PaymentHistory_Click(
            object? sender,
            EventArgs e)
        {
            PaymentHistoryForm historyForm =
                new PaymentHistoryForm(_userId);

            historyForm.FormClosed +=
                (s, args) => Show();

            Hide();

            historyForm.Show();
        }

        // =========================
        // WATER USAGE
        // =========================

        private void WaterUsage_Click(
            object? sender,
            EventArgs e)
        {
            MessageBox.Show(
                "Water Usage page will be connected next.",
                "NWSDB",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =========================
        // PROFILE
        // =========================

        private void Profile_Click(
            object? sender,
            EventArgs e)
        {
            ProfileForm profileForm =
                new ProfileForm(_userId);

            profileForm.FormClosed +=
                (s, args) => Show();

            Hide();

            profileForm.Show();
        }

        // =========================
        // LOGOUT
        // =========================

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

