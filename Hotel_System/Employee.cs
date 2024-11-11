using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Hotel_System
{
    /*
     * Class for insert/update/delete/get all employees
     */
    class Employee
    {
        DBConnection conn = new DBConnection();

        // Insert new employee
        public bool InsertEmployee(string firstName, string lastName, string phoneNumber,  string position,  decimal salary, DateTime hireDate)
        {
            MySqlCommand command = new MySqlCommand();
            string queryInsert = "INSERT INTO employees(first_name, last_name, phone_number, position,  salary, hire_date) VALUES (@firstName, @lastName, @phoneNumber, @position, @salary, @hireDate)";
            command.CommandText = queryInsert;
            command.Connection = conn.GetConnection();

            command.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = firstName;
            command.Parameters.Add("@lastName", MySqlDbType.VarChar).Value = lastName;
            command.Parameters.Add("@phoneNumber", MySqlDbType.VarChar).Value = phoneNumber;
            command.Parameters.Add("@position", MySqlDbType.VarChar).Value = position;
            command.Parameters.Add("@salary", MySqlDbType.Decimal).Value = salary; // Corrected to decimal
            command.Parameters.Add("@hireDate", MySqlDbType.Date).Value = hireDate; // Corrected to DateTime

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

        // Get all employees
        public DataTable GetAllEmployees()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM employees", conn.GetConnection());
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();

            adapter.SelectCommand = command;
            adapter.Fill(table);

            return table;
        }

        // Edit employee data
        public bool EditEmployee(int id, string firstName, string lastName, string phoneNumber, string position, decimal salary, DateTime hireDate)
        {
            MySqlCommand command = new MySqlCommand();
            string queryUpdate = "UPDATE employees SET first_name=@firstName, last_name=@lastName, phone_number=@phoneNumber, position=@position,  salary=@salary, hire_date=@hireDate WHERE id=@id";
            command.CommandText = queryUpdate;
            command.Connection = conn.GetConnection();

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = firstName;
            command.Parameters.Add("@lastName", MySqlDbType.VarChar).Value = lastName;
            command.Parameters.Add("@phoneNumber", MySqlDbType.VarChar).Value = phoneNumber;
            command.Parameters.Add("@position", MySqlDbType.VarChar).Value = position;
            command.Parameters.Add("@salary", MySqlDbType.Decimal).Value = salary;
            command.Parameters.Add("@hireDate", MySqlDbType.Date).Value = hireDate;

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

        public DataTable SearchEmployees(string firstName, string lastName)
{

            MySqlCommand command = new MySqlCommand();
            string querySearch = "SELECT * FROM employees WHERE first_name LIKE @firstName AND last_name LIKE @lastName";
            command.CommandText = querySearch;
            command.Connection = conn.GetConnection();
            command.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = $"%{firstName}%";
            command.Parameters.Add("@lastName", MySqlDbType.VarChar).Value = $"%{lastName}%";

        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        DataTable table = new DataTable();
        adapter.Fill(table);

        return table;
    }



        // Remove employee
        public bool RemoveEmployee(int id)
        {
            MySqlCommand command = new MySqlCommand();
            string queryDelete = "DELETE FROM employees WHERE id=@id";
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
    }
}
