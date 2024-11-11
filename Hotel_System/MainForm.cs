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
    public partial class MainForm : Form
    {
        private string userRole;
        private string username;

        public MainForm(string username, string role)
        {
            InitializeComponent();
            userRole = role;
            this.username = username;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void manageClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageClientsForm clients = new ManageClientsForm(userRole);
            clients.Show();
        }

        private void manageRoomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           ManageRoomsForm room = new ManageRoomsForm(userRole); 
            room.Show();
        }

        private void manageReservationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageReservationsForm reservation = new ManageReservationsForm();
            reservation.Show();
        }

        private void logOutbutton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }

        private void viewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewsForm views = new ViewsForm();
            views.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (userRole != "Admin")
            {
                manageEmployeesToolStripMenuItem.Visible = false;
            }
        }

        private void manageEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageEmployeeForm employees = new ManageEmployeeForm();
            employees.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ManageAccount account = new ManageAccount(username);
            account.Show();
        }
    }
}