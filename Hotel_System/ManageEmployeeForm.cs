using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_System
{
    public partial class ManageEmployeeForm : Form
    {
        Employee employee = new Employee();
        public ManageEmployeeForm()
        {
            InitializeComponent();
        }

        private bool ValidateNoNumbers(string input)
        {
            return !input.Any(char.IsDigit);
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+"))
            {
                phoneNumber = phoneNumber.Substring(1);
            }
            return phoneNumber.All(char.IsDigit);
        }

        private bool ValidateSalary(string salaryInput, out decimal salary)
        {
            return decimal.TryParse(salaryInput, out salary);
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            string firstName = tbFirstName.Text;
            string lastName = tbLastName.Text;
            string phone = tbPhone.Text;
            string position = tbPosition.Text;
            string salaryInput = tbSalary.Text;
            DateTime hireDate = dtpHireDate.Value;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(position))
            {
                MessageBox.Show("Required fields - Firstname/Lastname/Phone/Position", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!ValidateNoNumbers(firstName) || !ValidateNoNumbers(lastName))
            {
                MessageBox.Show("Firstname and Lastname should not contain numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!ValidatePhoneNumber(phone))
            {
                MessageBox.Show("Phone number should only contain digits, optionally starting with '+'", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!ValidateNoNumbers(position))
            {
                MessageBox.Show("Position should not contain numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!ValidateSalary(salaryInput, out decimal salary))
            {
                MessageBox.Show("Salary should be a valid number", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                bool isInsertedEmployee = employee.InsertEmployee(firstName, lastName, phone, position, salary, hireDate);

                if (isInsertedEmployee)
                {
                    dgvEmployees.DataSource = employee.GetAllEmployees();
                    MessageBox.Show("Employee inserted successfully!", "Employee Add", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnClearFields.PerformClick();
                }
                else
                {
                    MessageBox.Show("ERROR - Employee not inserted!", "Employee Add", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            int id;
            string firstName = tbFirstName.Text;
            string lastName = tbLastName.Text;
            string phone = tbPhone.Text;
            string position = tbPosition.Text;
            string salaryInput = tbSalary.Text;
            DateTime hireDate = dtpHireDate.Value;

            try
            {
                id = Convert.ToInt32(tbID.Text);

                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(position) || string.IsNullOrWhiteSpace(phone))
                {
                    MessageBox.Show("Required fields - Firstname/Lastname/Position/Phone", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!ValidateNoNumbers(firstName) || !ValidateNoNumbers(lastName))
                {
                    MessageBox.Show("Firstname and Lastname should not contain numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!ValidatePhoneNumber(phone))
                {
                    MessageBox.Show("Phone number should only contain digits, optionally starting with '+'", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!ValidateNoNumbers(position))
                {
                    MessageBox.Show("Position should not contain numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!ValidateSalary(salaryInput, out decimal salary))
                {
                    MessageBox.Show("Salary should be a valid number", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    bool isUpdatedEmployee = employee.EditEmployee(id, firstName, lastName, phone, position, salary, hireDate);

                    if (isUpdatedEmployee)
                    {
                        dgvEmployees.DataSource = employee.GetAllEmployees();
                        MessageBox.Show("Employee updated successfully!", "Employee Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("ERROR - Employee not updated!", "Employee Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ID error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click_1(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(tbID.Text);

                // Display a confirmation dialog
                DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (employee.RemoveEmployee(id))
                    {
                        dgvEmployees.DataSource = employee.GetAllEmployees();
                        MessageBox.Show("Employee deleted successfully!", "Employee Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnClearFields.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("ERROR - Employee not deleted!", "Employee Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ID error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearFields_Click_1(object sender, EventArgs e)
        {
            tbID.Text = "";
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbPhone.Text = "";
            tbPosition.Text = "";
            tbSalary.Text = "";
            dtpHireDate.Value = DateTime.Now;
        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            tbID.Text = dgvEmployees.CurrentRow.Cells[0].Value.ToString();
            tbFirstName.Text = dgvEmployees.CurrentRow.Cells[1].Value.ToString();
            tbLastName.Text = dgvEmployees.CurrentRow.Cells[2].Value.ToString();
            tbPhone.Text = dgvEmployees.CurrentRow.Cells[3].Value.ToString();
            tbPosition.Text = dgvEmployees.CurrentRow.Cells[4].Value.ToString();
            tbSalary.Text = dgvEmployees.CurrentRow.Cells[5].Value.ToString();
            dtpHireDate.Value = Convert.ToDateTime(dgvEmployees.CurrentRow.Cells[6].Value);
        }

        private void ManageEmployeeForm_Load(object sender, EventArgs e)
        {
            dgvEmployees.DataSource = employee.GetAllEmployees();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
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

            dgvEmployees.DataSource = employee.SearchEmployees(firstName, lastName);
        }
    }
}
