using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_System
{
    public partial class ManageClientsForm : Form
    {
        private string userRole;

        Client client = new Client();
        public ManageClientsForm(string role)
        {
            InitializeComponent();
            this.userRole = role;
        }


        private bool ValidatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+"))
            {
                phoneNumber = phoneNumber.Substring(1);
            }
            return phoneNumber.All(char.IsDigit);
        }

        private bool ValidateNoNumbers(string input)
        {
            return !input.Any(char.IsDigit);
        }



        private void btnClearFields_Click(object sender, EventArgs e)
        {
            tbID.Text = "";
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbPhone.Text = "";
            tbCountry.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            String fname = tbFirstName.Text;
            String lname = tbLastName.Text;
            String phone = tbPhone.Text;
            String country = tbCountry.Text;

            
            if (string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(country))
            {
                MessageBox.Show("Required fields - Firstname/Lastname/Phone/Country", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!ValidateNoNumbers(fname) || !ValidateNoNumbers(lname))
            {
                MessageBox.Show("Firstname and Lastname should not contain numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!ValidatePhoneNumber(phone))
            {
                MessageBox.Show("Phone number should only contain digits, optionally starting with '+'", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!ValidateNoNumbers(country))
            {
                MessageBox.Show("Country should not contain numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Boolean IsInsertedClient = client.InsertClient(fname, lname, phone, country);

                if (IsInsertedClient)
                {
                    dgvClients.DataSource = client.GetAllClients();
                    MessageBox.Show("Client inserted successfully!", "Client Add", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnClearFields.PerformClick();
                }
                else
                {
                    MessageBox.Show("ERROR - Client not inserted!", "Client Add", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
        }

        private void ManageClientsForm_Load(object sender, EventArgs e)
        {
            if (userRole != "Admin")
            {
                
                btnEdit.Visible = false;
                btnRemove.Visible = false;
            }

            dgvClients.DataSource = client.GetAllClients();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int id;
            String fname = tbFirstName.Text;
            String lname = tbLastName.Text;
            String phone = tbPhone.Text;
            String country = tbCountry.Text;

            try
            {
                id = Convert.ToInt32(tbID.Text);


                if (string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(country))
                {
                    MessageBox.Show("Required fields - Firstname/Lastname/Phone/Country", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!ValidateNoNumbers(fname) || !ValidateNoNumbers(lname))
                {
                    MessageBox.Show("Firstname and Lastname should not contain numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!ValidatePhoneNumber(phone))
                {
                    MessageBox.Show("Phone number should only contain digits, optionally starting with '+'", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!ValidateNoNumbers(country))
                {
                    MessageBox.Show("Country should not contain numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Boolean IsUpdatedClient = client.EditClient(id, fname, lname, phone, country);

                    if (IsUpdatedClient)
                    {
                        dgvClients.DataSource = client.GetAllClients();
                        MessageBox.Show("Client updated successfully!", "Client Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("ERROR - Client not updated!", "Client Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ID error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

         
        }


        private void dgvClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbID.Text = dgvClients.CurrentRow.Cells[0].Value.ToString();
            tbFirstName.Text = dgvClients.CurrentRow.Cells[1].Value.ToString();
            tbLastName.Text = dgvClients.CurrentRow.Cells[2].Value.ToString();
            tbPhone.Text = dgvClients.CurrentRow.Cells[3].Value.ToString();
            tbCountry.Text = dgvClients.CurrentRow.Cells[4].Value.ToString();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(tbID.Text);

                // Display a confirmation dialog
                DialogResult result = MessageBox.Show("Are you sure you want to delete this client?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (client.RemoveClient(id))
                    {
                        dgvClients.DataSource = client.GetAllClients();
                        MessageBox.Show("Client deleted successfully!", "Client Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnClearFields.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("ERROR - Client not deleted!", "Client Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ID error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            //search clients
            string[] names = textBox1.Text.Trim().Split(' ');

            string firstName = "";
            string lastName = "";

            if (names.Length > 0)
            {
                firstName = names[0];
                if (names.Length > 1)
                {
                    lastName = names[1];
                }
            }

            dgvClients.DataSource = client.SearchClient(firstName, lastName);
        }
    }
}
