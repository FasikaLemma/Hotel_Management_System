using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Hotel_System
{
    public partial class ViewsForm : Form
    {
        private DBConnection conn = new DBConnection();

        public ViewsForm()
        {
            InitializeComponent();
        }       
        
        private void LoadDataToDataGridView(string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, conn.GetConnection()))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvRooms.DataSource = table;
                }
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM view_reservations";
                LoadDataToDataGridView(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM view_room_details";
                LoadDataToDataGridView(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM view_reservation_summary";
                LoadDataToDataGridView(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM view_client_countries";
                LoadDataToDataGridView(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
