using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_System
{
    internal class Profile
    {
        DBConnection conn = new DBConnection();


        public bool UpdateUsername(string oldUsername, string newUsername)
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                string query = "UPDATE users SET username = @newUsername WHERE username = @oldUsername";
                command.CommandText = query;
                command.Connection = conn.GetConnection();

                command.Parameters.Add("@oldUsername", MySqlDbType.VarChar).Value = oldUsername;
                command.Parameters.Add("@newUsername", MySqlDbType.VarChar).Value = newUsername;

                conn.OpenConnection();
                bool result = command.ExecuteNonQuery() == 1;
                conn.CloseConnection();
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool UpdatePassword(string username, string newPassword)
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                string query = "UPDATE users SET password = @newPassword WHERE username = @username";
                command.CommandText = query;
                command.Connection = conn.GetConnection();

                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
                command.Parameters.Add("@newPassword", MySqlDbType.VarChar).Value = newPassword;

                conn.OpenConnection();
                bool result = command.ExecuteNonQuery() == 1;
                conn.CloseConnection();
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool VerifyCurrentPassword(string username, string currentPassword)
        {
            MySqlCommand command = new MySqlCommand();
            string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @currentPassword";
            command.CommandText = query;
            command.Connection = conn.GetConnection();

            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@currentPassword", MySqlDbType.VarChar).Value = currentPassword;

            conn.OpenConnection();
            int count = Convert.ToInt32(command.ExecuteScalar());
            conn.CloseConnection();

            return count == 1;
        }


    }
}
