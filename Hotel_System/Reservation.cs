using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace Hotel_System
{
    class Reservation
    {
        DBConnection conn = new DBConnection();
        //get all reservations
        public DataTable GetAllReservations()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM reservations", conn.GetConnection());
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();

            adapter.SelectCommand = command;
            adapter.Fill(table);

            return table;
        }

        public bool MakeReservation(int room, int client, DateTime dateIn, DateTime dateOut)
        {
            MySqlCommand command = new MySqlCommand();
            string queryInsert = "INSERT INTO `reservations`(`room_number`, `client_id`, `date_in`, `date_out`) VALUES (@room, @client, @dateIn, @dateOut)";
            command.CommandText = queryInsert;
            command.Connection = conn.GetConnection();

            command.Parameters.Add("@room", MySqlDbType.Int32).Value = room;
            command.Parameters.Add("@client", MySqlDbType.Int32).Value = client;
            command.Parameters.Add("@dateIn", MySqlDbType.Date).Value = dateIn;
            command.Parameters.Add("@dateOut", MySqlDbType.Date).Value = dateOut;

            conn.OpenConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                conn.CloseConnection();
                Room roomObj = new Room();
                roomObj.SetRoomFree(room, "NO");
                return true;
            }
            else
            {
                conn.CloseConnection();
                return false;
            }
        }

        // Edit reservation
        public bool EditReservation(int id, int room, int client, DateTime dateIn, DateTime dateOut)
        {

            MySqlCommand command = new MySqlCommand();
            string queryUpdate = "UPDATE `reservations` SET `room_number`=@room, `client_id`=@client, `date_in`=@dateIn, `date_out`=@dateOut WHERE id=@id";
            command.CommandText = queryUpdate;
            command.Connection = conn.GetConnection();

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@room", MySqlDbType.Int32).Value = room;
            command.Parameters.Add("@client", MySqlDbType.Int32).Value = client;
            command.Parameters.Add("@dateIn", MySqlDbType.Date).Value = dateIn;
            command.Parameters.Add("@dateOut", MySqlDbType.Date).Value = dateOut;

            conn.OpenConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                conn.CloseConnection();
                Room roomObj = new Room();
                roomObj.SetRoomFree(room, "NO");
                return true;
            }
            else
            {
                conn.CloseConnection();
                return false;
            }
        }
        //remove room
        public bool RemoveReservation(int id)
        {
            MySqlCommand command = new MySqlCommand();
            String queryDelete = "DELETE FROM reservations WHERE id=@id";
            command.CommandText = queryDelete;
            command.Connection = conn.GetConnection();

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            conn.OpenConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                conn.CloseConnection();
                return true;
            }
            else
            {
                conn.CloseConnection();
                return false;
            }
        }

        public decimal GetRoomTypePrice(int roomTypeId)
        {
            MySqlCommand command = new MySqlCommand("SELECT price FROM rooms_type WHERE id = @id", conn.GetConnection());
            command.Parameters.AddWithValue("@id", roomTypeId);
            conn.OpenConnection();
            object result = command.ExecuteScalar();
            conn.CloseConnection();
            return result != null ? Convert.ToDecimal(result) : 0m; // Return 0 if no price is found
        }



        public DataTable GetReservationsByRoomNumber(int room)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM reservations WHERE room_number = @room", conn.GetConnection());
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();

            command.Parameters.Add("@room", MySqlDbType.Int32).Value = room;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            return table;
        }

        public bool CheckRoomTypeAvailability(int type)
        {

            MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM rooms WHERE type=@type AND free = 'YES'", conn.GetConnection());
            command.Parameters.Add("@type", MySqlDbType.Int32).Value = type;

            conn.OpenConnection();
            int count = Convert.ToInt32(command.ExecuteScalar());
            conn.CloseConnection();

            Console.WriteLine($"CheckRoomTypeAvailability called for type {type}, available count: {count}");

            return count > 0;
        }

    }
}
