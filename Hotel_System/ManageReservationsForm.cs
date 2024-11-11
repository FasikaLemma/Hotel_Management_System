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
    public partial class ManageReservationsForm : Form
    {
        public ManageReservationsForm()
        {
            InitializeComponent();
        }

        Room room = new Room();
        Reservation reservation = new Reservation();
        private void ManageReservationsForm_Load(object sender, EventArgs e)
        {
            //display room's type
            cbRoomType.DataSource = room.RoomTypeList();
            cbRoomType.DisplayMember = "label";
            cbRoomType.ValueMember = "id";



            //display free room's number depending on selected type
            int type = Convert.ToInt32(cbRoomType.SelectedValue.ToString());
            cbRoomNumber.DataSource = room.RoomByType(type);
            cbRoomNumber.DisplayMember = "number";
            cbRoomNumber.ValueMember = "number";


            cbRoomType.SelectedIndex = -1;

            dgvReservations.DataSource = reservation.GetAllReservations();

        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            tbReservID.Text = "";
            tbClientID.Text = "";
            textBox2.Text = "";
            cbRoomType.SelectedIndex = -1;
            cbRoomNumber.SelectedIndex = -1;
            dateTimePickerIN.Value = DateTime.Now;
            dateTimePickerOUT.Value = DateTime.Now;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int clientID = Convert.ToInt32(tbClientID.Text);
                int roomNumber = Convert.ToInt32(cbRoomNumber.SelectedValue);
                DateTime dateIn = dateTimePickerIN.Value.Date;
                DateTime dateOut = dateTimePickerOUT.Value.Date;

                if (dateIn < DateTime.Today)
                {
                    MessageBox.Show("The Date must be Greater than Or Equal to the Current Date", "Invalid Date In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (dateOut < dateIn)
                {
                    MessageBox.Show("The DateOut must be Greater than Or Equal to the DateIn", "Invalid Date Out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (reservation.MakeReservation(roomNumber, clientID, dateIn, dateOut))
                    {
                        room.SetRoomFree(roomNumber, "NO");
                        dgvReservations.DataSource = reservation.GetAllReservations();
                        MessageBox.Show("Reservation made successfully!", "Make Reservation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnClearFields.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("ERROR - Reservation not added!", "Make Reservation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Make Reservation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int reservationID = Convert.ToInt32(tbReservID.Text);
                int clientID = Convert.ToInt32(tbClientID.Text);
                int roomNumber = Convert.ToInt32(dgvReservations.CurrentRow.Cells[1].Value.ToString());
                DateTime dateIn = dateTimePickerIN.Value.Date; 
                DateTime dateOut = dateTimePickerOUT.Value.Date;
                int room_type = Convert.ToInt32(cbRoomType.SelectedValue.ToString());

                if (!reservation.CheckRoomTypeAvailability(room_type))
                {
                    MessageBox.Show("There are no available rooms of the selected type. Please choose a different type.", "Room Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dateIn < DateTime.Today)
                {
                    MessageBox.Show("The Date must be Greater than Or Equal to the Current Date", "Invalid Date In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (dateOut < dateIn)
                {
                    MessageBox.Show("The DateOut must be Greater than Or Equal to the DateIn", "Invalid Date Out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (reservation.EditReservation(reservationID, roomNumber, clientID, dateIn, dateOut))
                    {
                        room.SetRoomFree(roomNumber, "NO");
                        dgvReservations.DataSource = reservation.GetAllReservations();
                        MessageBox.Show("Reservation data updated!", "Edit Reservation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("ERROR - Reservation not updated!", "Edit Reservation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                   

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Edit Reservation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // Prompt user for confirmation before deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this reservation?", "Delete Reservation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    int reservationID = Convert.ToInt32(tbReservID.Text);
                    int roomNumber = Convert.ToInt32(dgvReservations.CurrentRow.Cells[1].Value.ToString());

                    if (reservation.RemoveReservation(reservationID))
                    {
                        room.SetRoomFree(roomNumber, "YES");
                        dgvReservations.DataSource = reservation.GetAllReservations();
                        MessageBox.Show("Reservation deleted successfully!", "Reservation Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnClearFields.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("ERROR - Reservation not deleted!", "Reservation Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete reservation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbRoomType.SelectedValue != null && int.TryParse(cbRoomType.SelectedValue.ToString(), out int type))
                {
                    // Display room's number depending on selected type
                    cbRoomNumber.DataSource = room.RoomByType(type);
                    cbRoomNumber.DisplayMember = "number";
                    cbRoomNumber.ValueMember = "number";

                    // Fetch and display the initial price based on the selected room type
                    DataTable roomTypeTable = room.GetRoomTypePrice(type);
                    if (roomTypeTable.Rows.Count > 0)
                    {
                        textBox2.Text = roomTypeTable.Rows[0]["price"].ToString();
                    }
                    else
                    {
                        textBox2.Text = "0.00"; // Set default value if no price is available
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void CalculateTotalPrice()
        {
            if (cbRoomType.SelectedValue != null && int.TryParse(cbRoomType.SelectedValue.ToString(), out int type))
            {
                decimal roomTypePrice = reservation.GetRoomTypePrice(type);
                DateTime dateIn = dateTimePickerIN.Value;
                DateTime dateOut = dateTimePickerOUT.Value;

                // Calculate the total days of the reservation
                TimeSpan totalDays = dateOut - dateIn;
                if (totalDays.TotalDays > 0) // Check if the total days are positive
                {
                    decimal totalPrice = (decimal)totalDays.TotalDays * roomTypePrice;
                    textBox2.Text = totalPrice.ToString("F2");
                }
                else
                {
                    textBox2.Text = "0.00"; // Set default value if the total days are not positive
                }
            }
        }

        private void dgvReservations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvReservations.CurrentRow != null)
                {
                    tbReservID.Text = dgvReservations.CurrentRow.Cells[0].Value?.ToString();

                    if (int.TryParse(dgvReservations.CurrentRow.Cells[1].Value?.ToString(), out int roomID))
                    {
                        cbRoomType.SelectedValue = room.GetRoomType(roomID);
                        cbRoomNumber.SelectedValue = roomID;
                    }
                    else
                    {
                        MessageBox.Show("Invalid room ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Clears the selected index
                        cbRoomType.SelectedIndex = -1;
                        cbRoomNumber.SelectedIndex = -1;
                    }

                    tbClientID.Text = dgvReservations.CurrentRow.Cells[2].Value?.ToString();

                    if (DateTime.TryParse(dgvReservations.CurrentRow.Cells[3].Value?.ToString(), out DateTime dateIn))
                    {
                        dateTimePickerIN.Value = dateIn;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Date In", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dateTimePickerIN.Value = DateTime.Now;
                    }

                    if (DateTime.TryParse(dgvReservations.CurrentRow.Cells[4].Value?.ToString(), out DateTime dateOut))
                    {
                        dateTimePickerOUT.Value = dateOut;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Date Out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dateTimePickerOUT.Value = DateTime.Now;
                    }

                    // Calculate the total price when a cell is clicked
                    CalculateTotalPrice();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int roomNumber;
            if (int.TryParse(textBox1.Text, out roomNumber))
            {
                dgvReservations.DataSource = reservation.GetReservationsByRoomNumber(roomNumber);
            }
            else
            {
                dgvReservations.DataSource = reservation.GetAllReservations();
            }
        }

        private void dateTimePickerIN_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }

        private void dateTimePickerOUT_ValueChanged(object sender, EventArgs e)
        {
            // Check if the check-in date is today's date
            bool checkInToday = dateTimePickerIN.Value.Date == DateTime.Today;

            // If the check-in date is today's date or earlier, recalculate the total price
            if (checkInToday || dateTimePickerOUT.Value > dateTimePickerIN.Value)
            {
                CalculateTotalPrice();
            }
        }

    }
}