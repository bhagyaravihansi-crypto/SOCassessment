using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace NWSDBDesktopApp
{
    public partial class LoginForm : Form
    {
        private readonly HttpClient _httpClient;

        private TextBox emailBox = null!;
        private TextBox passwordBox = null!;
        private Button loginButton = null!;
        private Button exitButton = null!;

        public LoginForm()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7293/")
            };

            BuildLoginUI();
        }

        private void BuildLoginUI()
        {
            Text = "NWSDB - Login";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(900, 550);
            MinimumSize = new Size(800, 500);
            BackColor = Color.White;

            // LEFT PANEL

            Panel leftPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(400, ClientSize.Height),
                BackColor = Color.FromArgb(0, 91, 170),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Bottom |
                         AnchorStyles.Left
            };

            Controls.Add(leftPanel);

            Label logoTitle = new Label
            {
                Text = "NWSDB",
                Location = new Point(55, 130),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font(
                    "Segoe UI",
                    32,
                    FontStyle.Bold)
            };

            leftPanel.Controls.Add(logoTitle);

            Label organization = new Label
            {
                Text = "National Water Supply\r\n" +
                       "and Drainage Board",
                Location = new Point(60, 195),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font(
                    "Segoe UI",
                    15)
            };

            leftPanel.Controls.Add(organization);

            Label welcome = new Label
            {
                Text = "Water Services Management System",
                Location = new Point(60, 275),
                AutoSize = true,
                ForeColor = Color.FromArgb(220, 235, 250),
                Font = new Font(
                    "Segoe UI",
                    10)
            };

            leftPanel.Controls.Add(welcome);

            // RIGHT PANEL

            Panel rightPanel = new Panel
            {
                Location = new Point(400, 0),
                Size = new Size(
                    ClientSize.Width - 400,
                    ClientSize.Height),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Bottom |
                         AnchorStyles.Left |
                         AnchorStyles.Right
            };

            Controls.Add(rightPanel);

            Label loginTitle = new Label
            {
                Text = "Welcome Back",
                Location = new Point(65, 75),
                AutoSize = true,
                ForeColor = Color.FromArgb(30, 50, 80),
                Font = new Font(
                    "Segoe UI",
                    24,
                    FontStyle.Bold)
            };

            rightPanel.Controls.Add(loginTitle);

            Label subtitle = new Label
            {
                Text = "Sign in to continue to NWSDB",
                Location = new Point(68, 120),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    10)
            };

            rightPanel.Controls.Add(subtitle);

            // EMAIL

            Label emailLabel = new Label
            {
                Text = "Email",
                Location = new Point(68, 175),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50)
            };

            rightPanel.Controls.Add(emailLabel);

            emailBox = new TextBox
            {
                Location = new Point(68, 205),
                Size = new Size(350, 35),
                Font = new Font(
                    "Segoe UI",
                    11)
            };

            rightPanel.Controls.Add(emailBox);

            // PASSWORD

            Label passwordLabel = new Label
            {
                Text = "Password",
                Location = new Point(68, 255),
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50)
            };

            rightPanel.Controls.Add(passwordLabel);

            passwordBox = new TextBox
            {
                Location = new Point(68, 285),
                Size = new Size(350, 35),
                Font = new Font(
                    "Segoe UI",
                    11),
                UseSystemPasswordChar = true
            };

            rightPanel.Controls.Add(passwordBox);

            // LOGIN BUTTON

            loginButton = new Button
            {
                Text = "Login",
                Location = new Point(68, 350),
                Size = new Size(165, 45),
                BackColor = Color.FromArgb(0, 91, 170),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.Click += LoginButton_Click;

            rightPanel.Controls.Add(loginButton);

            // EXIT BUTTON

            exitButton = new Button
            {
                Text = "Exit",
                Location = new Point(253, 350),
                Size = new Size(165, 45),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(0, 91, 170),
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            exitButton.FlatAppearance.BorderColor =
                Color.FromArgb(0, 91, 170);

            exitButton.Click += ExitButton_Click;

            rightPanel.Controls.Add(exitButton);

            // ENTER KEY

            emailBox.KeyDown += LoginInput_KeyDown;
            passwordBox.KeyDown += LoginInput_KeyDown;
        }

        // ENTER KEY

        private void LoginInput_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                LoginButton_Click(
                    loginButton,
                    EventArgs.Empty);
            }
        }

        // LOGIN

        private async void LoginButton_Click(
            object? sender,
            EventArgs e)
        {
            string email = emailBox.Text.Trim();
            string password = passwordBox.Text;

            // VALIDATE EMAIL

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(
                    "Please enter your email.",
                    "NWSDB Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                emailBox.Focus();
                return;
            }

            // VALIDATE PASSWORD

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Please enter your password.",
                    "NWSDB Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                passwordBox.Focus();
                return;
            }

            try
            {
                loginButton.Enabled = false;
                loginButton.Text = "Signing in...";

                // LOGIN DATA

                var loginData = new
                {
                    Email = email,
                    Password = password
                };

                string json =
                    JsonSerializer.Serialize(loginData);

                // SEND REQUEST
               

                using StringContent content =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage response =
                    await _httpClient.PostAsync(
                        "api/Auth/login",
                        content);

                string responseJson =
                    await response.Content.ReadAsStringAsync();

                
                // LOGIN FAILED
                

                if (!response.IsSuccessStatusCode)
                {
                    string message =
                        "Invalid email or password.";

                    try
                    {
                        using JsonDocument document =
                            JsonDocument.Parse(responseJson);

                        if (document.RootElement.TryGetProperty(
                            "message",
                            out JsonElement messageElement))
                        {
                            message =
                                messageElement.GetString()
                                ?? message;
                        }
                    }
                    catch
                    {
                        // Use default message.
                    }

                    MessageBox.Show(
                        message,
                        "NWSDB Login",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
  
                // LOGIN SUCCESS
               

                using JsonDocument result =
                    JsonDocument.Parse(responseJson);

                int userId =
                    result.RootElement
                        .GetProperty("userId")
                        .GetInt32();

                string role =
                    result.RootElement
                        .GetProperty("role")
                        .GetString()
                        ?? "Customer";

               
                // SUCCESS MESSAGE
               

                MessageBox.Show(
                    "Login successful!",
                    "NWSDB",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

               
                // OPEN DASHBOARD BY ROLE
               

                Hide();

                if (role.Equals(
                    "Admin",
                    StringComparison.OrdinalIgnoreCase))
                {
                  
                    // ADMIN
                   

                    MainForm mainForm =
                        new MainForm();

                    mainForm.FormClosed +=
                        (s, args) => Close();

                    mainForm.Show();
                }
                else
                {
                    
                    // CUSTOMER
                  

                    CustomerDashboardForm
                        customerDashboard =
                            new CustomerDashboardForm(userId);

                    customerDashboard.FormClosed +=
                        (s, args) => Close();

                    customerDashboard.Show();
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show(
                    "Unable to connect to the NWSDB API.\n\n" +
                    "Please make sure NWSDB.API is running.",
                    "NWSDB Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (JsonException)
            {
                MessageBox.Show(
                    "The NWSDB API returned an invalid response.",
                    "NWSDB Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An unexpected error occurred.\n\n" +
                    ex.Message,
                    "NWSDB Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                loginButton.Enabled = true;
                loginButton.Text = "Login";
            }
        }

        
        // EXIT
     

        private void ExitButton_Click(
            object? sender,
            EventArgs e)
        {
            Application.Exit();
        }
    }
}