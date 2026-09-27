using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using NWSDB.BusinessLogic.DTOs;

namespace NWSDBDesktopApp
{
    public partial class OnlinePaymentForm : Form
    {
        private readonly int _userId;
        private readonly HttpClient _httpClient;

        private CustomerDto? _customer;

        private Label customerNameLabel = null!;
        private Label accountNumberLabel = null!;
        private TextBox amountTextBox = null!;
        private ComboBox paymentMethodComboBox = null!;

        // Card fields
        private Label cardNumberLabel = null!;
        private TextBox cardNumberTextBox = null!;

        private Label cardHolderLabel = null!;
        private TextBox cardHolderTextBox = null!;

        private Label expiryLabel = null!;
        private TextBox expiryTextBox = null!;

        private Label cvvLabel = null!;
        private TextBox cvvTextBox = null!;

        private Button payButton = null!;
        private Label statusLabel = null!;

        public OnlinePaymentForm(int userId)
        {
            _userId = userId;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7293/")
            };

            BuildPaymentUI();

            LoadCustomerDataAsync();
        }

        private void BuildPaymentUI()
        {
            Text = "NWSDB Online Payment";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(800, 700);
            MinimumSize = new Size(700, 650);
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

            // MAIN PANEL

            Panel paymentPanel = new Panel
            {
                Location = new Point(150, 90),
                Size = new Size(500, 550),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            Controls.Add(paymentPanel);

            // TITLE

            Label paymentTitle = new Label
            {
                Text = "Online Payment",
                Location = new Point(30, 20),
                AutoSize = true,
                ForeColor = Color.FromArgb(30, 50, 80),
                Font = new Font(
                    "Segoe UI",
                    20,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(paymentTitle);

            // CUSTOMER NAME

            Label customerNameText = new Label
            {
                Text = "Customer Name",
                Location = new Point(30, 70),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(customerNameText);

            customerNameLabel = new Label
            {
                Text = "Loading...",
                Location = new Point(200, 70),
                AutoSize = true,
                ForeColor = Color.FromArgb(30, 50, 80),
                Font = new Font(
                    "Segoe UI",
                    10)
            };

            paymentPanel.Controls.Add(customerNameLabel);

            // ACCOUNT NUMBER

            Label accountNumberText = new Label
            {
                Text = "Account Number",
                Location = new Point(30, 105),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(accountNumberText);

            accountNumberLabel = new Label
            {
                Text = "Loading...",
                Location = new Point(200, 105),
                AutoSize = true,
                ForeColor = Color.FromArgb(30, 50, 80),
                Font = new Font(
                    "Segoe UI",
                    10)
            };

            paymentPanel.Controls.Add(accountNumberLabel);

            // PAYMENT AMOUNT

            Label amountLabel = new Label
            {
                Text = "Payment Amount (LKR)",
                Location = new Point(30, 150),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(amountLabel);

            amountTextBox = new TextBox
            {
                Location = new Point(30, 177),
                Size = new Size(430, 32),
                Font = new Font(
                    "Segoe UI",
                    12),
                PlaceholderText = "Enter payment amount"
            };

            paymentPanel.Controls.Add(amountTextBox);

            // PAYMENT METHOD

            Label methodLabel = new Label
            {
                Text = "Payment Method",
                Location = new Point(30, 220),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(methodLabel);

            paymentMethodComboBox = new ComboBox
            {
                Location = new Point(30, 247),
                Size = new Size(430, 32),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font(
                    "Segoe UI",
                    11)
            };

            paymentMethodComboBox.Items.Add("Card");
            paymentMethodComboBox.Items.Add("Mobile");
            paymentMethodComboBox.Items.Add("Bank");

            paymentMethodComboBox.SelectedIndex = 0;

            paymentMethodComboBox.SelectedIndexChanged +=
                PaymentMethodComboBox_SelectedIndexChanged;

            paymentPanel.Controls.Add(
                paymentMethodComboBox);

            // CARD NUMBER

            cardNumberLabel = new Label
            {
                Text = "Card Number",
                Location = new Point(30, 295),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(cardNumberLabel);

            cardNumberTextBox = new TextBox
            {
                Location = new Point(30, 318),
                Size = new Size(430, 30),
                Font = new Font(
                    "Segoe UI",
                    10),
                PlaceholderText = "1234 5678 9012 3456",
                MaxLength = 19
            };

            paymentPanel.Controls.Add(cardNumberTextBox);

            // CARD HOLDER

            cardHolderLabel = new Label
            {
                Text = "Card Holder Name",
                Location = new Point(30, 355),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(cardHolderLabel);

            cardHolderTextBox = new TextBox
            {
                Location = new Point(30, 378),
                Size = new Size(430, 30),
                Font = new Font(
                    "Segoe UI",
                    10),
                PlaceholderText = "Name on card"
            };

            paymentPanel.Controls.Add(cardHolderTextBox);

            // EXPIRY DATE

            expiryLabel = new Label
            {
                Text = "Expiry Date",
                Location = new Point(30, 415),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(expiryLabel);

            expiryTextBox = new TextBox
            {
                Location = new Point(30, 438),
                Size = new Size(150, 30),
                Font = new Font(
                    "Segoe UI",
                    10),
                PlaceholderText = "MM/YY",
                MaxLength = 5
            };

            paymentPanel.Controls.Add(expiryTextBox);

            // CVV

            cvvLabel = new Label
            {
                Text = "CVV",
                Location = new Point(205, 415),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(cvvLabel);

            cvvTextBox = new TextBox
            {
                Location = new Point(205, 438),
                Size = new Size(120, 30),
                Font = new Font(
                    "Segoe UI",
                    10),
                PlaceholderText = "123",
                MaxLength = 3,
                UseSystemPasswordChar = true
            };

            paymentPanel.Controls.Add(cvvTextBox);

            // STATUS

            statusLabel = new Label
            {
                Text = "",
                Location = new Point(30, 475),
                Size = new Size(430, 25),
                ForeColor = Color.Red,
                Font = new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold)
            };

            paymentPanel.Controls.Add(statusLabel);

            // PAY BUTTON
   

            payButton = new Button
            {
                Text = "Pay Now",
                Location = new Point(30, 505),
                Size = new Size(430, 40),
                BackColor = Color.FromArgb(0, 91, 170),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI",
                    11,
                    FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            payButton.FlatAppearance.BorderSize = 0;

            payButton.Click += PayButton_Click;

            paymentPanel.Controls.Add(payButton);
        }

      
        // CARD FIELDS SHOW / HIDE
   

        private void PaymentMethodComboBox_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            bool isCard =
                paymentMethodComboBox.SelectedItem?.ToString()
                == "Card";

            cardNumberLabel.Visible = isCard;
            cardNumberTextBox.Visible = isCard;

            cardHolderLabel.Visible = isCard;
            cardHolderTextBox.Visible = isCard;

            expiryLabel.Visible = isCard;
            expiryTextBox.Visible = isCard;

            cvvLabel.Visible = isCard;
            cvvTextBox.Visible = isCard;
        }

        // LOAD CUSTOMER
       

        private async void LoadCustomerDataAsync()
        {
            try
            {
                _customer =
                    await _httpClient.GetFromJsonAsync<CustomerDto>(
                        $"api/Customers/by-user/{_userId}");

                if (_customer == null)
                {
                    customerNameLabel.Text = "Not found";
                    accountNumberLabel.Text = "Not found";

                    payButton.Enabled = false;

                    MessageBox.Show(
                        "Customer account was not found.",
                        "NWSDB",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                customerNameLabel.Text =
                    string.IsNullOrWhiteSpace(_customer.FullName)
                        ? "Not available"
                        : _customer.FullName;

                accountNumberLabel.Text =
                    string.IsNullOrWhiteSpace(_customer.AccountNumber)
                        ? "Not available"
                        : _customer.AccountNumber;
            }
            catch (HttpRequestException)
            {
                payButton.Enabled = false;

                MessageBox.Show(
                    "Unable to connect to NWSDB API.\n\n" +
                    "Please make sure NWSDB.API is running.",
                    "NWSDB Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                payButton.Enabled = false;

                MessageBox.Show(
                    "Unable to load customer information.\n\n" +
                    ex.Message,
                    "NWSDB Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

     
        // PAY BUTTON
      

        private async void PayButton_Click(
            object? sender,
            EventArgs e)
        {
            statusLabel.Text = "";
            statusLabel.ForeColor = Color.Red;

            if (_customer == null)
            {
                MessageBox.Show(
                    "Customer information is not available.",
                    "NWSDB",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

    
            // VALIDATE AMOUNT
   
            if (!decimal.TryParse(
                    amountTextBox.Text.Trim(),
                    out decimal amount))
            {
                statusLabel.Text =
                    "Please enter a valid payment amount.";

                return;
            }

            if (amount <= 0)
            {
                statusLabel.Text =
                    "Payment amount must be greater than LKR 0.00.";

                return;
            }

            string paymentMethod =
                paymentMethodComboBox.SelectedItem?.ToString()
                ?? "Card";

            // CARD VALIDATION

            if (paymentMethod == "Card")
            {
                string cardNumber =
                    cardNumberTextBox.Text
                        .Replace(" ", "")
                        .Trim();

                if (cardNumber.Length != 16 ||
                    !long.TryParse(cardNumber, out _))
                {
                    statusLabel.Text =
                        "Please enter a valid 16-digit card number.";

                    return;
                }

                if (string.IsNullOrWhiteSpace(
                        cardHolderTextBox.Text))
                {
                    statusLabel.Text =
                        "Please enter the card holder name.";

                    return;
                }

                string expiry =
                    expiryTextBox.Text.Trim();

                if (expiry.Length != 5 ||
                    expiry[2] != '/')
                {
                    statusLabel.Text =
                        "Please enter expiry date as MM/YY.";

                    return;
                }

                string cvv =
                    cvvTextBox.Text.Trim();

                if (cvv.Length != 3 ||
                    !int.TryParse(cvv, out _))
                {
                    statusLabel.Text =
                        "Please enter a valid 3-digit CVV.";

                    return;
                }
            }

            payButton.Enabled = false;
            payButton.Text = "Processing...";

            try
            {
                // CREATE PAYMENT REQUEST

                var payment = new PaymentCreateDto
                {
                    CustomerId = _customer.Id,
                    BillId = null,
                    Amount = amount,
                    PaymentMethod = paymentMethod
                };

                HttpResponseMessage response =
                    await _httpClient.PostAsJsonAsync(
                        "api/Payments",
                        payment);

                if (response.IsSuccessStatusCode)
                {
                    PaymentDto? result =
                        await response.Content
                            .ReadFromJsonAsync<PaymentDto>();

                    string reference =
                        result?.PaymentReference
                        ?? "Generated";

                    MessageBox.Show(
                        "Payment Successful!\n\n" +
                        "Amount: LKR " +
                        amount.ToString("N2") +
                        "\n" +
                        "Payment Method: " +
                        paymentMethod +
                        "\n" +
                        "Reference: " +
                        reference,
                        "NWSDB Payment",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    amountTextBox.Clear();

                    cardNumberTextBox.Clear();
                    cardHolderTextBox.Clear();
                    expiryTextBox.Clear();
                    cvvTextBox.Clear();

                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text =
                        "Payment completed successfully.";
                }
                else
                {
                    string error =
                        await response.Content
                            .ReadAsStringAsync();

                    statusLabel.ForeColor = Color.Red;
                    statusLabel.Text =
                        "Payment failed.";

                    MessageBox.Show(
                        "Payment could not be completed.\n\n" +
                        error,
                        "NWSDB Payment Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show(
                    "Unable to connect to NWSDB API.\n\n" +
                    "Please make sure NWSDB.API is running.",
                    "NWSDB Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred while processing payment.\n\n" +
                    ex.Message,
                    "NWSDB Payment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                payButton.Enabled = true;
                payButton.Text = "Pay Now";
            }
        }

        private void OnlinePaymentForm_Load(object sender, EventArgs e)
        {

        }
    }
}