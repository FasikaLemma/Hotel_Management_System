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
    public partial class ManageRoomsForm : Form
    {
        private string userRole;

        public ManageRoomsForm(string role)
        {
            InitializeComponent();
            this.userRole = role;
        }
        Room room = new Room();

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+"))
            {
                phoneNumber = phoneNumber.Substring(1);
            }
            return phoneNumber.All(char.IsDigit);
        }

        private bool ValidateRoomNumber(string roomNumber)
        {
            // Check if all characters are digits
            return roomNumber.All(char.IsDigit);
        }

        private void ManageRoomsForm_Load(object sender, EventArgs e)
        {
            if (userRole != "Admin")
            {

                btnEdit.Visible = false;
                btnRemove.Visible = false;
                btnAdd.Visible = false;
                btnClearFields.Visible = false;
                tbNumber.Visible = false;
               // cbRoomType.Visible = false;
                tbPhone.Visible = false;
                label2.Visible = false;
               // label3.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                panel3.Visible = false;
            }
            cbRoomType.DataSource = room.RoomTypeList();
            cbRoomType.DisplayMember = "label";
            cbRoomType.ValueMember = "id";

            cbRoomType.SelectedIndex = -1;

            dgvRooms.DataSource = room.GetAllRooms();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {

                // Validate room number
                string roomNumberStr = tbNumber.Text;
                if (!ValidateRoomNumber(roomNumberStr))
                {
                    MessageBox.Show("Room number should only contain digits", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int type = Convert.ToInt32(cbRoomType.SelectedValue.ToString());
                String phone = tbPhone.Text;
                String free = "";


                // Validate phone number
                if (!ValidatePhoneNumber(phone))
                {
                    MessageBox.Show("Phone number should only contain digits, optionally starting with '+'", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                int number = Convert.ToInt32(tbNumber.Text);
                if (rbYes.Checked)
                {
                    free = "YES";
                }
                else if (rbNo.Checked)
                {
                    free = "NO";
                }

                if (room.InsertRoom(number, type, phone, free))
                {
                    dgvRooms.DataSource = room.GetAllRooms();
                    MessageBox.Show("Room inserted successfully!", "Room Add", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnClearFields.PerformClick();
                }
                else
                {
                    MessageBox.Show("ERROR - Room not inserted!", "Room Add", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Room number error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            try
            {
                // Validate room number
                string roomNumberStr = tbNumber.Text;
                
                
                int number = Convert.ToInt32(roomNumberStr);
                int type = Convert.ToInt32(cbRoomType.SelectedValue.ToString());
                string phone = tbPhone.Text;
                string free = "";


                if (rbYes.Checked)
                {
                    free = "YES";
                }
                else if (rbNo.Checked)
                {
                    free = "NO";
                }



                if (room.EditRoom(number, type, phone, free))
                {
                    dgvRooms.DataSource = room.GetAllRooms();
                    MessageBox.Show("Room data updated successfully!", "Room Update", MessageBoxButtons.OK, MessageBoxIcon.Information);                   
                }
                else
                {
                    MessageBox.Show("ERROR - Room data not updated!", "Room Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Room number error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int number = Convert.ToInt32(tbNumber.Text);

                // Display a confirmation dialog
                DialogResult result = MessageBox.Show("Are you sure you want to delete this room?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (room.RemoveRoom(number))
                    {
                        dgvRooms.DataSource = room.GetAllRooms();
                        MessageBox.Show("Room deleted successfully!", "Room Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnClearFields.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("ERROR - Room not deleted!", "Room Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Room number error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            tbNumber.Text = "";
            cbRoomType.SelectedIndex = -1;
            tbPhone.Text = "";
            textBox2.Text = "";
            rbYes.Checked = true;
        }


        private void dgvRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbNumber.Text = dgvRooms.CurrentRow.Cells[0].Value.ToString();
            cbRoomType.SelectedValue = dgvRooms.CurrentRow.Cells[1].Value;
            tbPhone.Text = dgvRooms.CurrentRow.Cells[2].Value.ToString();

            string free = dgvRooms.CurrentRow.Cells[3].Value.ToString();

            if (free.Equals("YES"))
            {
                rbYes.Checked = true;
                //rbNo.Checked = false;
            }
            else if (free.Equals("NO"))
            {
                // rbYes.Checked = false;
                rbNo.Checked = true;
            }
            //get the price
            int type = Convert.ToInt32(cbRoomType.SelectedValue.ToString());
            DataTable roomTypeTable = room.GetRoomTypePrice(type);
            if (roomTypeTable.Rows.Count > 0)
            {
                textBox2.Text = roomTypeTable.Rows[0]["price"].ToString();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(searchValue))
            {

                int roomNumber = Convert.ToInt32(searchValue);
                DataTable filteredTable = room.SearchRoom(roomNumber);
                dgvRooms.DataSource = filteredTable;
            }
            else
            {
                dgvRooms.DataSource = room.GetAllRooms();
            }
        }

        private void cbRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Attempt to convert the selected value to an integer
                if (int.TryParse(cbRoomType.SelectedValue?.ToString(), out int type))
                {
                    // Fetch and display the price based on the selected room type
                    DataTable roomTypeTable = room.GetRoomTypePrice(type);
                    if (roomTypeTable.Rows.Count > 0)
                    {
                        textBox2.Text = roomTypeTable.Rows[0]["price"].ToString();
                    }
                }
                else
                {
                    // Handle the case where the selected value is not a valid integer
                    textBox2.Text = ""; // Clear the price textbox or display an error message
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Room type error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}