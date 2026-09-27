using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using NWSDB.BusinessLogic.DTOs;

namespace NWSDBDesktopApp
{
    public partial class ProfileForm : Form
    {
        private readonly int _userId;
        private readonly HttpClient _httpClient;

        private Label headerNameLabel = null!;
        private Label headerAccountLabel = null!;

        private Label fullNameValue = null!;
        private Label accountNumberValue = null!;
        private Label nicValue = null!;
        private Label emailValue = null!;
        private Label phoneValue = null!;
        private Label addressValue = null!;
        private Label statusValue = null!;

        public ProfileForm(int userId)
        {
            InitializeComponent();

            _userId = userId;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7293/")
            };

            BuildProfilePage();

            Load += ProfileForm_Load;
        }

        // FORM LOAD

        private async void ProfileForm_Load(
            object? sender,
            EventArgs e)
        {
            await LoadProfileAsync();
        }

        // BUILD PROFILE PAGE

        private void BuildProfilePage()
        {
            Text = "NWSDB - My Profile";

            StartPosition =
                FormStartPosition.CenterScreen;

            ClientSize =
                new Size(1150, 720);

            MinimumSize =
                new Size(1000, 650);

            BackColor = Color.White;

            // MAIN LAYOUT

            TableLayoutPanel mainLayout =
                new TableLayoutPanel();

            mainLayout.Dock =
                DockStyle.Fill;

            mainLayout.ColumnCount = 2;
            mainLayout.RowCount = 2;

            mainLayout.Margin =
                new Padding(0);

            mainLayout.Padding =
                new Padding(0);

            mainLayout.BackColor =
                Color.White;

            // Sidebar width
            mainLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    220));

            // Content remaining width
            mainLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    100));

            // Header height
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    80));

            // Remaining page
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            // HEADER

            Panel header =
                new Panel();

            header.Dock =
                DockStyle.Fill;

            header.BackColor =
                Color.FromArgb(
                    0,
                    83,
                    156);

            header.Margin =
                new Padding(0);

            header.Padding =
                new Padding(0);

            // HEADER LEFT

            TableLayoutPanel headerLeft =
                new TableLayoutPanel();

            headerLeft.Dock =
                DockStyle.Left;

            headerLeft.Width = 300;

            headerLeft.ColumnCount = 1;
            headerLeft.RowCount = 2;

            headerLeft.Margin =
                new Padding(0);

            headerLeft.Padding =
                new Padding(
                    25,
                    8,
                    0,
                    5);

            headerLeft.BackColor =
                Color.Transparent;

            headerLeft.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    38));

            headerLeft.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    25));

            Label logoLabel =
                new Label();

            logoLabel.Text =
                "NWSDB";

            logoLabel.Dock =
                DockStyle.Fill;

            logoLabel.Font =
                new Font(
                    "Segoe UI",
                    17,
                    FontStyle.Bold);

            logoLabel.ForeColor =
                Color.White;

            logoLabel.AutoSize = false;

            logoLabel.TextAlign =
                ContentAlignment.MiddleLeft;

            logoLabel.Margin =
                new Padding(0);

            Label portalLabel =
                new Label();

            portalLabel.Text =
                "Customer Portal";

            portalLabel.Dock =
                DockStyle.Fill;

            portalLabel.Font =
                new Font(
                    "Segoe UI",
                    9);

            portalLabel.ForeColor =
                Color.White;

            portalLabel.AutoSize = false;

            portalLabel.TextAlign =
                ContentAlignment.MiddleLeft;

            portalLabel.Margin =
                new Padding(0);

            headerLeft.Controls.Add(
                logoLabel,
                0,
                0);

            headerLeft.Controls.Add(
                portalLabel,
                0,
                1);

            header.Controls.Add(
                headerLeft);

            // HEADER RIGHT

            TableLayoutPanel headerRight =
                new TableLayoutPanel();

            headerRight.Dock =
                DockStyle.Right;

            headerRight.Width = 350;

            headerRight.ColumnCount = 1;
            headerRight.RowCount = 2;

            headerRight.Margin =
                new Padding(0);

            headerRight.Padding =
                new Padding(
                    0,
                    10,
                    25,
                    5);

            headerRight.BackColor =
                Color.Transparent;

            headerRight.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    30));

            headerRight.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    25));

            headerNameLabel =
                new Label();

            headerNameLabel.Text =
                "Customer";

            headerNameLabel.Dock =
                DockStyle.Fill;

            headerNameLabel.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold);

            headerNameLabel.ForeColor =
                Color.White;

            headerNameLabel.AutoSize = false;

            headerNameLabel.TextAlign =
                ContentAlignment.MiddleRight;

            headerNameLabel.Margin =
                new Padding(0);

            headerAccountLabel =
                new Label();

            headerAccountLabel.Text =
                "Account";

            headerAccountLabel.Dock =
                DockStyle.Fill;

            headerAccountLabel.Font =
                new Font(
                    "Segoe UI",
                    9);

            headerAccountLabel.ForeColor =
                Color.White;

            headerAccountLabel.AutoSize = false;

            headerAccountLabel.TextAlign =
                ContentAlignment.MiddleRight;

            headerAccountLabel.Margin =
                new Padding(0);

            headerRight.Controls.Add(
                headerNameLabel,
                0,
                0);

            headerRight.Controls.Add(
                headerAccountLabel,
                0,
                1);

            header.Controls.Add(
                headerRight);

            // ADD HEADER

            mainLayout.Controls.Add(
                header,
                0,
                0);

            mainLayout.SetColumnSpan(
                header,
                2);

            // SIDEBAR

            Panel sidebar =
                new Panel();

            sidebar.Dock =
                DockStyle.Fill;

            sidebar.BackColor =
                Color.FromArgb(
                    247,
                    249,
                    252);

            sidebar.Margin =
                new Padding(0);

            sidebar.Padding =
                new Padding(
                    0,
                    15,
                    0,
                    0);

            // MENU TITLE

            Label menuTitle =
                new Label();

            menuTitle.Text =
                "CUSTOMER MENU";

            menuTitle.Dock =
                DockStyle.Top;

            menuTitle.Height = 30;

            menuTitle.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold);

            menuTitle.ForeColor =
                Color.Gray;

            menuTitle.AutoSize = false;

            menuTitle.TextAlign =
                ContentAlignment.MiddleLeft;

            menuTitle.Padding =
                new Padding(
                    20,
                    0,
                    0,
                    0);

            sidebar.Controls.Add(
                menuTitle);

            // MENU

            TableLayoutPanel menuTable =
                new TableLayoutPanel();

            menuTable.Dock =
                DockStyle.Top;

            menuTable.Height = 315;

            menuTable.ColumnCount = 1;
            menuTable.RowCount = 7;

            menuTable.Margin =
                new Padding(0);

            menuTable.Padding =
                new Padding(0);

            for (int i = 0; i < 7; i++)
            {
                menuTable.RowStyles.Add(
                    new RowStyle(
                        SizeType.Absolute,
                        45));
            }

            Button dashboardButton =
                CreateMenuButton("Dashboard");

            Button currentBillButton =
                CreateMenuButton("Current Bill");

            Button onlinePaymentButton =
                CreateMenuButton("Online Payment");

            Button paymentHistoryButton =
                CreateMenuButton("Payment History");

            Button waterUsageButton =
                CreateMenuButton("Water Usage");

            Button profileButton =
                CreateMenuButton("Profile");

            Button logoutButton =
                CreateMenuButton("Logout");

            // Active Profile
            profileButton.BackColor =
                Color.FromArgb(
                    220,
                    235,
                    250);

            profileButton.ForeColor =
                Color.FromArgb(
                    0,
                    83,
                    156);

            menuTable.Controls.Add(
                dashboardButton,
                0,
                0);

            menuTable.Controls.Add(
                currentBillButton,
                0,
                1);

            menuTable.Controls.Add(
                onlinePaymentButton,
                0,
                2);

            menuTable.Controls.Add(
                paymentHistoryButton,
                0,
                3);

            menuTable.Controls.Add(
                waterUsageButton,
                0,
                4);

            menuTable.Controls.Add(
                profileButton,
                0,
                5);

            menuTable.Controls.Add(
                logoutButton,
                0,
                6);

            sidebar.Controls.Add(
                menuTable);

            // MENU EVENTS

            dashboardButton.Click +=
                DashboardButton_Click;

            onlinePaymentButton.Click +=
                OnlinePaymentButton_Click;

            paymentHistoryButton.Click +=
                PaymentHistoryButton_Click;

            profileButton.Click +=
                ProfileButton_Click;

            logoutButton.Click +=
                LogoutButton_Click;

            // ADD SIDEBAR
            

            mainLayout.Controls.Add(
                sidebar,
                0,
                1);

            // CONTENT

            Panel content =
                new Panel();

            content.Dock =
                DockStyle.Fill;

            content.BackColor =
                Color.White;

            content.Margin =
                new Padding(0);

            content.Padding =
                new Padding(
                    30,
                    25,
                    30,
                    25);

            // CONTENT LAYOUT

            TableLayoutPanel contentLayout =
                new TableLayoutPanel();

            contentLayout.Dock =
                DockStyle.Fill;

            contentLayout.ColumnCount = 1;
            contentLayout.RowCount = 3;

            contentLayout.Margin =
                new Padding(0);

            contentLayout.Padding =
                new Padding(0);

            contentLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    40));

            contentLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    30));

            contentLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            // TITLE

            Label titleLabel =
                new Label();

            titleLabel.Text =
                "My Profile";

            titleLabel.Dock =
                DockStyle.Fill;

            titleLabel.Font =
                new Font(
                    "Segoe UI",
                    20,
                    FontStyle.Bold);

            titleLabel.ForeColor =
                Color.FromArgb(
                    35,
                    35,
                    35);

            titleLabel.AutoSize = false;

            titleLabel.TextAlign =
                ContentAlignment.MiddleLeft;

            titleLabel.Margin =
                new Padding(0);

            contentLayout.Controls.Add(
                titleLabel,
                0,
                0);

            // SUBTITLE

            Label subtitleLabel =
                new Label();

            subtitleLabel.Text =
                "View your NWSDB customer account information.";

            subtitleLabel.Dock =
                DockStyle.Fill;

            subtitleLabel.Font =
                new Font(
                    "Segoe UI",
                    9);

            subtitleLabel.ForeColor =
                Color.Gray;

            subtitleLabel.AutoSize = false;

            subtitleLabel.TextAlign =
                ContentAlignment.MiddleLeft;

            subtitleLabel.Margin =
                new Padding(0);

            contentLayout.Controls.Add(
                subtitleLabel,
                0,
                1);

            // PROFILE CARD

            Panel profileCard =
                new Panel();

            profileCard.Dock =
                DockStyle.Fill;

            profileCard.BackColor =
                Color.White;

            profileCard.BorderStyle =
                BorderStyle.FixedSingle;

            profileCard.Margin =
                new Padding(
                    0,
                    15,
                    0,
                    0);

            profileCard.Padding =
                new Padding(25);

            // CARD LAYOUT
            

            TableLayoutPanel cardLayout =
                new TableLayoutPanel();

            cardLayout.Dock =
                DockStyle.Fill;

            cardLayout.ColumnCount = 1;
            cardLayout.RowCount = 2;

            cardLayout.Margin =
                new Padding(0);

            cardLayout.Padding =
                new Padding(0);

            cardLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    40));

            cardLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            // CARD TITLE

            Label cardTitle =
                new Label();

            cardTitle.Text =
                "Customer Information";

            cardTitle.Dock =
                DockStyle.Fill;

            cardTitle.Font =
                new Font(
                    "Segoe UI",
                    14,
                    FontStyle.Bold);

            cardTitle.ForeColor =
                Color.FromArgb(
                    30,
                    50,
                    80);

            cardTitle.AutoSize = false;

            cardTitle.TextAlign =
                ContentAlignment.MiddleLeft;

            cardTitle.Margin =
                new Padding(0);

            cardLayout.Controls.Add(
                cardTitle,
                0,
                0);

            // INFORMATION TABLE

            TableLayoutPanel infoTable =
                new TableLayoutPanel();

            infoTable.Dock =
                DockStyle.Fill;

            infoTable.ColumnCount = 4;
            infoTable.RowCount = 4;

            infoTable.Margin =
                new Padding(
                    0,
                    10,
                    0,
                    0);

            infoTable.Padding =
                new Padding(0);

            // Columns
            infoTable.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    120));

            infoTable.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    35));

            infoTable.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    130));

            infoTable.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    65));

            // Rows
            for (int i = 0; i < 4; i++)
            {
                infoTable.RowStyles.Add(
                    new RowStyle(
                        SizeType.Absolute,
                        65));
            }

            // INFORMATION

            AddInfoRow(
                infoTable,
                0,
                0,
                "Full Name",
                out fullNameValue);

            AddInfoRow(
                infoTable,
                2,
                0,
                "Account Number",
                out accountNumberValue);

            AddInfoRow(
                infoTable,
                0,
                1,
                "NIC",
                out nicValue);

            AddInfoRow(
                infoTable,
                2,
                1,
                "Email",
                out emailValue);

            AddInfoRow(
                infoTable,
                0,
                2,
                "Phone",
                out phoneValue);

            AddInfoRow(
                infoTable,
                2,
                2,
                "Status",
                out statusValue);

            AddInfoRow(
                infoTable,
                0,
                3,
                "Address",
                out addressValue);

            // ADD INFORMATION TABLE

            cardLayout.Controls.Add(
                infoTable,
                0,
                1);

            profileCard.Controls.Add(
                cardLayout);

            // ADD PROFILE CARD

            contentLayout.Controls.Add(
                profileCard,
                0,
                2);

            // ADD CONTENT

            content.Controls.Add(
                contentLayout);

            mainLayout.Controls.Add(
                content,
                1,
                1);

            // ADD MAIN LAYOUT

            Controls.Add(
                mainLayout);

            mainLayout.BringToFront();
        }

        // ADD INFORMATION ROW

        private void AddInfoRow(
            TableLayoutPanel table,
            int column,
            int row,
            string title,
            out Label valueLabel)
        {
            Label titleLabel =
                new Label();

            titleLabel.Text =
                title;

            titleLabel.Dock =
                DockStyle.Fill;

            titleLabel.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold);

            titleLabel.ForeColor =
                Color.Gray;

            titleLabel.AutoSize = false;

            titleLabel.TextAlign =
                ContentAlignment.MiddleLeft;

            titleLabel.Margin =
                new Padding(0);

            valueLabel =
                new Label();

            valueLabel.Text =
                "Loading...";

            valueLabel.Dock =
                DockStyle.Fill;

            valueLabel.Font =
                new Font(
                    "Segoe UI",
                    10);

            valueLabel.ForeColor =
                Color.FromArgb(
                    35,
                    35,
                    35);

            valueLabel.AutoSize = false;

            valueLabel.TextAlign =
                ContentAlignment.MiddleLeft;

            valueLabel.Margin =
                new Padding(
                    0,
                    0,
                    15,
                    0);

            valueLabel.Padding =
                new Padding(
                    5,
                    0,
                    5,
                    0);

            table.Controls.Add(
                titleLabel,
                column,
                row);

            table.Controls.Add(
                valueLabel,
                column + 1,
                row);
        }

        // CREATE MENU BUTTON

        private Button CreateMenuButton(
            string text)
        {
            Button button =
                new Button();

            button.Text =
                text;

            button.Dock =
                DockStyle.Fill;

            button.FlatStyle =
                FlatStyle.Flat;

            button.BackColor =
                Color.Transparent;

            button.ForeColor =
                Color.FromArgb(
                    50,
                    50,
                    50);

            button.Font =
                new Font(
                    "Segoe UI",
                    10);

            button.TextAlign =
                ContentAlignment.MiddleLeft;

            button.Padding =
                new Padding(
                    20,
                    0,
                    5,
                    0);

            button.Margin =
                new Padding(
                    10,
                    1,
                    10,
                    1);

            button.Cursor =
                Cursors.Hand;

            button.FlatAppearance.BorderSize =
                0;

            return button;
        }

        // LOAD PROFILE DATA

        private async Task LoadProfileAsync()
        {
            try
            {
                Cursor =
                    Cursors.WaitCursor;

                CustomerDto? customer =
                    await _httpClient
                        .GetFromJsonAsync<CustomerDto>(
                            $"api/Customers/by-user/{_userId}");

                if (customer == null)
                {
                    MessageBox.Show(
                        "Customer profile could not be found.",
                        "NWSDB",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // Header
                headerNameLabel.Text =
                    customer.FullName;

                headerAccountLabel.Text =
                    "Account: " +
                    customer.AccountNumber;

                // Profile
                fullNameValue.Text =
                    GetValue(customer.FullName);

                accountNumberValue.Text =
                    GetValue(customer.AccountNumber);

                nicValue.Text =
                    GetValue(customer.NIC);

                emailValue.Text =
                    GetValue(customer.Email);

                phoneValue.Text =
                    GetValue(customer.Phone);

                addressValue.Text =
                    GetValue(customer.Address);

                statusValue.Text =
                    GetValue(customer.Status);

                if (string.Equals(
                    customer.Status,
                    "Active",
                    StringComparison.OrdinalIgnoreCase))
                {
                    statusValue.ForeColor =
                        Color.Green;
                }
                else
                {
                    statusValue.ForeColor =
                        Color.OrangeRed;
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show(
                    "Unable to connect to NWSDB API.\n\n" +
                    "Please make sure the API is running.",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load customer profile.\n\n" +
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

        // SAFE VALUE

        private string GetValue(
            string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "Not available";
            }

            return value;
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
                (s, args) => Show();

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
                (s, args) => Show();

            Hide();

            paymentForm.Show();
        }

        // PAYMENT HISTORY

        private void PaymentHistoryButton_Click(
            object? sender,
            EventArgs e)
        {
            PaymentHistoryForm historyForm =
                new PaymentHistoryForm(
                    _userId);

            historyForm.FormClosed +=
                (s, args) => Show();

            Hide();

            historyForm.Show();
        }

        // PROFILE

        private void ProfileButton_Click(
            object? sender,
            EventArgs e)
        {
            // Already on Profile page.
        }

        // LOGOUT

        private void LogoutButton_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to logout?",
                    "NWSDB Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result ==
                DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // CLOSE

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            _httpClient.Dispose();

            base.OnFormClosed(e);
        }
    }
}