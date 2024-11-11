using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Hotel_System
{
    public partial class ManageAccount : Form
    {
        Profile profile = new Profile();
        string signedInUsername;

        public ManageAccount(string username)
        {
            InitializeComponent();
            signedInUsername = username;
           
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //show edit username panel
            panel2.Visible = true;
            panel1.Visible = false;
   
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //clear field of manage account
            tbPassword.Clear();
            textBox1.Clear();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            //edit password button
            panel1.Visible = true;
            panel2.Visible = false;
          
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //cancel password
            panel1.Visible = false;
            panel2.Visible = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //cancel username
            panel1.Visible = false;
            panel2.Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //clear username textbox tbusername
            tbUsername.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //edit username
            string newUsername = tbUsername.Text;


            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("New username cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (profile.UpdateUsername(signedInUsername, newUsername))
            {
                MessageBox.Show("User profile updated successfully!", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                signedInUsername = newUsername;
            }
            else
            {
                MessageBox.Show("Failed to update username", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //edit password
            string currentPassword = tbPassword.Text;
            string newPassword = textBox1.Text;
            string confirmPassword = textBox2.Text;

            // Verify current password
            if (!profile.VerifyCurrentPassword(signedInUsername, currentPassword))
            {
                tbPassword.Clear();
                textBox1.Clear();
                textBox2.Clear();
                MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            // Check if new password matches confirm password
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirmed password do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
                textBox2.Clear();
                return;
            }

            // Update password
            if (profile.UpdatePassword(signedInUsername, newPassword))
            {
                MessageBox.Show("Password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error updating password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
