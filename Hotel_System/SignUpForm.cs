using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Hotel_System
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any fields are empty
                if (tbUsername.Text.Trim().Equals("") || tbPassword.Text.Trim().Equals("") || textBox1.Text.Trim().Equals("") || comboBox1.Text.Trim().Equals(""))
                {
                    MessageBox.Show("All fields are required", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if passwords match
                if (!tbPassword.Text.Equals(textBox1.Text))
                {
                    MessageBox.Show("Passwords do not match", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // If role is admin, ask for verification code
                if (comboBox1.Text.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    string verificationCode = ShowInputDialog("Enter verification code:");

                    if (verificationCode != "1111")
                    {
                        MessageBox.Show("Incorrect verification code", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Return to sign-up form
                    }
                }

                // Initialize connection and command
                DBConnection conn = new DBConnection();
                MySqlCommand command = new MySqlCommand();
                String query = "INSERT INTO users (username, password, role) VALUES (@username, @password, @role);";

                command.CommandText = query;
                command.Connection = conn.GetConnection();

                // Add parameters
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = tbUsername.Text;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = tbPassword.Text;
                command.Parameters.Add("@role", MySqlDbType.VarChar).Value = comboBox1.Text;

                // Open connection, execute command, and close connection
                conn.GetConnection().Open();
                int result = command.ExecuteNonQuery();
                conn.GetConnection().Close();

                // Check if the insertion was successfull
                if (result > 0)
                {
                    MessageBox.Show("Account successfully created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Navigate to login page
                    this.Hide();
                    LoginForm loginForm = new LoginForm();
                    loginForm.Show();
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Failed to create account. Please try again.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                DialogResult dialogResult = MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

            }
        }

        private string ShowInputDialog(string prompt)
        {
            Form promptForm = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Verification Required",
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label() { Left = 50, Top = 20, Width = 200, Text = prompt };
            TextBox inputBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
            Button confirmation = new Button() { Text = "OK", Left = 150, Top = 80, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { promptForm.Close(); };

            promptForm.Controls.Add(textLabel);
            promptForm.Controls.Add(inputBox);
            promptForm.Controls.Add(confirmation);
            promptForm.AcceptButton = confirmation;

            return promptForm.ShowDialog() == DialogResult.OK ? inputBox.Text : string.Empty;
        }
    }
}
